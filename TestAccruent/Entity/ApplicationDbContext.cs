using TestAccruent.Model;
using Microsoft.EntityFrameworkCore;

namespace TestAccruent.Entity
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {            
        }

        // Registered DB Model in ApplicationDbContext file
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setting a primary key in product model
            modelBuilder.Entity<Product>().HasKey(x => x.Id);

            // Inserting record in stock table
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Product1",
                    Code = "1"
                }, new Product
                {
                    Id = 2,
                    Name = "Product2",
                    Code = "2"
                }, new Product
                {
                    Id = 3,
                    Name = "Product3",
                    Code = "3"
                }, new Product
                {
                    Id = 4,
                    Name = "Product4",
                    Code = "4"
                }
            );
        }
    }
}