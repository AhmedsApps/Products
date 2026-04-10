using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Models;
using Products.Server.Controllers;
using Products.Server.Data;

namespace Products.Test
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetProducts_GetAllProducts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            using var context = new AppDbContext(options);
            context.Products.Add(new Product { Id = 1, Colour = "Red", ProductName = "Product1", ProductPrice = 100 });
            context.Products.Add(new Product { Id = 2, Colour = "Blue", ProductName = "Product2", ProductPrice = 200 });
            context.SaveChanges();
            var controller = new ProductsController(context);
            // Act
            var result = controller.GetProducts(null) as OkObjectResult;
            // Assert
            Assert.NotNull(result);
            var products = result.Value as List<Product>;
            Assert.NotNull(products);
            Assert.Equal(2, products.Count);
        }
    }  
}
