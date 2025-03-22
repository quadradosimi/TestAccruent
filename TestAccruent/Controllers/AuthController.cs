using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAccruent.Model;
using TestAccruent.Service;

namespace TestAccruent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration config, IAuthService authService)
        {
            _config = config;
            _authService = authService;
        }

        [HttpPost("loginJwt")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            var root = _config.GetSection("JWT");
            var userName = root.GetSection("UserName").Value;
            var password = root.GetSection("Password").Value;

            if (user.Username == userName && user.Password == password)
            {
                var token = GenerateJwtToken(user.Username);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //jwt
            var secret = _config.GetSection("JWT");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret.GetSection("Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddDays(365000),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Post([FromBody] UserLogin loginObject)
        //{
        //    var stock = await _authService.GetStockAuth(loginObject);

        //    if (stock?.Password != loginObject.Password)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok(new
        //    {
        //        data = stock,
        //        token = stock!.Id,
        //        ok = true
        //    });
        //}
    }
}
