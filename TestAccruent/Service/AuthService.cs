using TestAccruent.Entity;
using TestAccruent.Model;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace TestAccruent.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        public AuthService(ApplicationDbContext db)
        {
            _db = db;
        }

        //public async Task<Stock?> GetStockAuth(UserLogin loginObject)
        //{
        //    return await _db.Stock.FirstOrDefaultAsync(stock => stock.Email == loginObject.Username);
        //}

    }
}
