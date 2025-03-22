using TestAccruent.Entity;
using TestAccruent.Model;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TestAccruent.Service
{
    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _db;
        public StockService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Stock>> GetAllStocks(bool? isActive)
        {
            if (isActive == null) { return await _db.Stock.ToListAsync(); }

            return await _db.Stock.ToListAsync();
        }

        public async Task<Stock?> GetStockByID(int id)
        {
            return await _db.Stock.FirstOrDefaultAsync(stock => stock.Id == id);
        }

        public async Task<Stock?> AddStock(Stock obj)
        {
            var stock = new Stock()
            {
                ProductId = obj.ProductId,
                Type = obj.Type,
                CreatedAt = DateTime.Now,
                Quantity = obj.Quantity
            };

            _db.Stock.Add(stock);
            var result = await _db.SaveChangesAsync();
            return result >= 0 ? stock : null;
        }

        public async Task<Stock?> UpdateStock(int id, Stock obj)
        {
            var stock = await _db.Stock.FirstOrDefaultAsync(index => index.Id == id);
            if (stock != null)
            {
                stock.ProductId = obj.ProductId;
                stock.Type = obj.Type;
                stock.CreatedAt = obj.CreatedAt;
                stock.Quantity = obj.Quantity;

                var result = await _db.SaveChangesAsync();
                return result >= 0 ? stock : null;
            }
            return null;
        }

        public async Task<bool> DeleteStockByID(int id)
        {
            var stock = await _db.Stock.FirstOrDefaultAsync(index => index.Id == id);
            if (stock != null)
            {
                _db.Stock.Remove(stock);
                var result = await _db.SaveChangesAsync();
                return result >= 0;
            }
            return false;
        }

        public async Task<bool> ValidateNegativeStockForProduct(string idProduct, int currentQuantity)
        {

            var result = new List<ReportStockBalanceByProductResponse>();

            result = _db.Stock.Where(g => g.ProductId == Convert.ToInt64(idProduct))
                    .GroupBy(x => x.ProductId)
                    .Select(x => new ReportStockBalanceByProductResponse()
                    {
                        ProductCode = x.Key,
                        TotalCredits = x.Where(b => b.Type == "entrada").Sum(s => s.Quantity),
                        TotalDebits = x.Where(b => b.Type == "saida").Sum(s => s.Quantity),
                    })
                    .ToList();

            var totalBalance = Convert.ToInt64(result.FirstOrDefault().TotalCredits) - Convert.ToInt64(result.FirstOrDefault().TotalDebits);

            if (totalBalance - currentQuantity < 0)
            {
                return false;
            }

            return true;

        }
    }
}
