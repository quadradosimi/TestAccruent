using TestAccruent.Entity;
using TestAccruent.Model;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Microsoft.AspNetCore.Http.HttpResults;
using static Azure.Core.HttpHeader;

namespace TestAccruent.Service
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _db;
        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Product>> Get()
        {
            return await _db.Product.ToListAsync();
        }

        public async Task<bool> ValidateIfProductExist(string idProduct)
        {
            var product =  await _db.Product.FirstOrDefaultAsync(product => product.Id == Convert.ToInt64(idProduct));

            if (product.Id > 0)
            {
                return true;
            }

            return false;

        }
    }
}
