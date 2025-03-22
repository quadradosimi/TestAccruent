using TestAccruent.Entity;
using TestAccruent.Model;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Microsoft.AspNetCore.Http.HttpResults;
using static Azure.Core.HttpHeader;

namespace TestAccruent.Service
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _db;
        public ReportService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<ReportStockBalanceByProductResponse>> GetStockBalance()
        {
            var result = _db.Stock.GroupBy(x => x.ProductId)
            .Select(x => new  ReportStockBalanceByProductResponse()
            {
                ProductCode = x.Key,
                TotalCredits = x.Where(b => b.Type == "entrada").Sum(s => s.Quantity),
                TotalDebits = x.Where(b => b.Type == "saida").Sum(s => s.Quantity),
            })
            .ToList();
            
            var finalResult = result
                .GroupJoin(_db.Product,
                    r => r.ProductCode,
                    p => p.Id,
                    (stock, prod) => new { stock, prod })
                .SelectMany(
                    g => g.prod.DefaultIfEmpty(),
                    (a, e) => new ReportStockBalanceByProductResponse()
                    {
                        ProductName = a.prod.FirstOrDefault().Name,
                        ProductCode = a.stock.ProductCode,
                        TotalDebits = a.stock.TotalDebits,
                        TotalCredits = a.stock.TotalCredits,
                        TotalBalance = a.stock.TotalCredits - a.stock.TotalDebits
                    }).ToList();

            return finalResult;

        }

        public async Task<List<ReportStockBalanceByProductResponse>> GetStockBalanceFilter(ReportStockBalanceByProductRequest filter)
        {
            var finalMovementDate = filter.MovementDate.AddHours(23).AddMinutes(59);

            var result = new List<ReportStockBalanceByProductResponse>();

            if (filter.ProductId == "")
            {
                 result = _db.Stock.Where(f => f.CreatedAt > filter.MovementDate && f.CreatedAt < finalMovementDate)
                        .GroupBy(x => x.ProductId)
                        .Select(x => new ReportStockBalanceByProductResponse()
                        {
                            ProductCode = x.Key,
                            TotalCredits = x.Where(b => b.Type == "entrada").Sum(s => s.Quantity),
                            TotalDebits = x.Where(b => b.Type == "saida").Sum(s => s.Quantity),
                        })
                        .ToList();
            }
            else
            {
                result = _db.Stock.Where(f => f.CreatedAt > filter.MovementDate && f.CreatedAt < finalMovementDate)
                    .Where(g => g.ProductId == Convert.ToInt64(filter.ProductId))
                     .GroupBy(x => x.ProductId)
                     .Select(x => new ReportStockBalanceByProductResponse()
                     {
                         ProductCode = x.Key,
                         TotalCredits = x.Where(b => b.Type == "entrada").Sum(s => s.Quantity),
                         TotalDebits = x.Where(b => b.Type == "saida").Sum(s => s.Quantity),
                     })
                     .ToList();
            }

                var finalResult = result
                    .GroupJoin(_db.Product,
                        r => r.ProductCode,
                        p => p.Id,
                        (stock, prod) => new { stock, prod })
                    .SelectMany(
                        g => g.prod.DefaultIfEmpty(),
                        (a, e) => new ReportStockBalanceByProductResponse()
                        {
                            ProductName = a.prod.FirstOrDefault().Name,
                            ProductCode = a.stock.ProductCode,
                            TotalDebits = a.stock.TotalDebits,
                            TotalCredits = a.stock.TotalCredits,
                            TotalBalance = a.stock.TotalCredits - a.stock.TotalDebits
                        }).ToList();

            return finalResult;

        }        
    }
}
