
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;


namespace P3AddNewFunctionalityDotNetCore.Tests
{

    public class IntegrationTests01
    {

        //Arrange

        private readonly IStringLocalizer<ProductService>? _localizer;
        private P3Referential? context;
        private ProductService? productService;
        private ProductController? productController;


        public IntegrationTests01()
        {
            // Get the connection string for the test database
            var projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appConfigTest.json")
                .Build();

            var connectionString = configuration.GetConnectionString("TestP3Referential");

            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString).Options;

            // Initialization 
            context = new P3Referential(options, configuration);
            Cart cart = new();
            ProductRepository productRepository = new(context);
            OrderRepository orderRepository = new(context);

            // Initialize the StringLocalizer
            var localizationOptions = Options.Create(new LocalizationOptions());
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var factory = new ResourceManagerStringLocalizerFactory(localizationOptions, loggerFactory);
            var localizer = new StringLocalizer<ProductService>(factory);

            productService = new(cart, productRepository, orderRepository, localizer);
            LanguageService languageService = new();
            productController = new(productService, languageService, localizer);
        }


        [Fact]
        public async Task SaveNewProduct()
        {
            //Arrange        

            ProductViewModel expectedProduct = new() { Name = "toto", Description = "Description ", Details = "Detail", Stock = "10", Price = "10 " };

            //Get the number of products in the database
            int count = productService.GetAllProducts().Count();


            //Act
            productController.Create(expectedProduct);


            //Assert
            ////Verify if Product.Count has been incremented 
            Assert.Equal(count + 1, productService.GetAllProducts().Count());

            //Search if the product exists in the database
           // var result = await context.Product.Where(x => x.Name == "toto").FirstOrDefaultAsync();
            var result = productService.GetAllProducts().Where(x => x.Name == "toto").FirstOrDefault();
            Assert.NotNull(result);
            Assert.True(result.Name == expectedProduct.Name);
            Assert.True(result.Description == expectedProduct.Description);
            Assert.True(result.Details == expectedProduct.Details);
            Assert.True(result.Quantity == int.Parse(expectedProduct.Stock));
            Assert.True(result.Price == double.Parse(expectedProduct.Price));


            // Cleaning Database to reset the state
            context.Product.Remove(result);
            await context.SaveChangesAsync();
        }



        [Fact]
        public async Task DeleteProduct()
        {
            //Arrange        

            ProductViewModel productViewModel = new() { Name = "titi", Description = "Description ", Details = "Detail", Stock = "10", Price = "10 " };

            //Get the number of products in the database
            int count = productService.GetAllProducts().Count();

            //Create a product to delete
            productController.Create(productViewModel);
            var product = productService.GetAllProducts().Where(x => x.Name == "titi").FirstOrDefault();


            //Act
            //Delete the product

            productService.DeleteProduct(product.Id);

            //Assert
            //Verify if Product.Count has been decremented 
            Assert.Equal(count, productService.GetAllProducts().Count());

            //Search the product in the database
            //var productDontExistsAnymore = await context.Product.Where(p => p.Name == "titi").FirstOrDefaultAsync();
            var productDontExistsAnymore = productService.GetAllProducts().Where(x => x.Name == "titi").FirstOrDefault();

            //Verify if the Product has been deleted
            Assert.Null(productDontExistsAnymore);

        }

    }
}