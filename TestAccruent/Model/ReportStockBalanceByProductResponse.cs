using Microsoft.EntityFrameworkCore;

namespace TestAccruent.Model
{
    public class ReportStockBalanceByProductResponse
    {
        public int ProductCode { get; set; }
        public string ProductName { get; set; }
        public int TotalDebits { get; set; }
        public int TotalCredits { get; set; }
        public int TotalBalance { get; set; }

    }
}


