using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Testing.Platform.Configurations;
using System;
using System.Runtime.InteropServices;
using TestAccruent.Controllers;
using TestAccruent.Entity;
using TestAccruent.Model;
using TestAccruent.Service;

namespace TestProject2
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public async Task PostStock()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<StockController>();

            Stock stock = new Stock();

            stock.ProductId = 1;
            stock.Type = "entrada";
            stock.CreatedAt = DateTime.Now;
            stock.Quantity = 1;

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Stock;Trusted_Connection=True;TrustServerCertificate=true;")
            .Options;

            ApplicationDbContext context = new ApplicationDbContext(options);

            StockService stockService = new StockService(context);

            StockController addStock = new StockController(stockService, logger);
            var result = await addStock.Post(stock);

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public async Task GetAllStocks()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<StockController>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Stock;Trusted_Connection=True;TrustServerCertificate=true;")
            .Options;

            ApplicationDbContext context = new ApplicationDbContext(options);

            StockService stockService = new StockService(context);

            StockController addStock = new StockController(stockService, logger);
            var result = await addStock.Get();

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public async Task GetAllProducts()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<ProductController>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Stock;Trusted_Connection=True;TrustServerCertificate=true;")
            .Options;

            ApplicationDbContext context = new ApplicationDbContext(options);

            ProductService productService = new ProductService(context);

            ProductController getProducts = new ProductController(productService, logger);
            var result = await getProducts.Get();

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public async Task GetReportStockBalance()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<ReportController>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Stock;Trusted_Connection=True;TrustServerCertificate=true;")
            .Options;

            ApplicationDbContext context = new ApplicationDbContext(options);

            ReportService reportService = new ReportService(context);

            ReportController getReport = new ReportController(reportService, logger);
            var result = await getReport.GetStockBalance("1");

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public async Task StockBalanceValidation()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<StockController>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Stock;Trusted_Connection=True;TrustServerCertificate=true;")
            .Options;

            ApplicationDbContext context = new ApplicationDbContext(options);

            StockService stockService = new StockService(context);

            StockController addStock = new StockController(stockService, logger);
            var result = await addStock.ValidateNegativeStockForProduct("1;3");

            Assert.IsNotNull(result);

        }
    }
}
