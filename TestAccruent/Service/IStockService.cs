using Microsoft.AspNetCore.Mvc;
using TestAccruent.Model;

namespace TestAccruent.Service
{
    public interface IStockService
    {
        Task<List<Stock>> GetAllStocks(bool? isActive);
        Task<Stock?> GetStockByID(int id);
        Task<Stock?> AddStock(Stock obj);
        Task<Stock?> UpdateStock(int id, Stock obj);
        Task<bool> DeleteStockByID(int id);
        Task<bool> ValidateNegativeStockForProduct(string idProduct, int currentQuantity);
    }
}
