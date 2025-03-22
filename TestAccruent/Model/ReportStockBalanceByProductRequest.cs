using Microsoft.EntityFrameworkCore;

namespace TestAccruent.Model
{
    public class ReportStockBalanceByProductRequest
    {
        public DateTime MovementDate { get; set; }
        public string? ProductId { get; set; }

    }
}


