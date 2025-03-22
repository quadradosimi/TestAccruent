using Microsoft.EntityFrameworkCore;

namespace TestAccruent.Model
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        //public Product Product { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }

    }
}


