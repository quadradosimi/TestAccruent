using TestAccruent.Model;

namespace TestAccruent.Service
{
    public interface IReportService
    {
        Task<List<ReportStockBalanceByProductResponse>> GetStockBalanceFilter(ReportStockBalanceByProductRequest filter);
        Task<List<ReportStockBalanceByProductResponse>> GetStockBalance();
    }
}
