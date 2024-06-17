
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using NuGet.ContentModel;
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
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<ProductService> _localizer;


        [Fact]
        public async Task SaveNewProduct()
        {
            //Arrange        
            var options = new DbContextOptionsBuilder<P3Referential>()
             .UseSqlServer("Data Source=.;Initial Catalog=TestBase;Integrated Security=True").Options;

            P3Referential context = new(options, _configuration);
            LanguageService languageService = new();
            Cart cart = new();
            ProductRepository productRepository = new(context);
            OrderRepository orderRepository = new(context);
            ProductService productService = new(cart, productRepository, orderRepository, _localizer);
            ProductController productController = new(productService, languageService);
            ProductViewModel productViewModel = new() { Name = "toto", Description = "Description ", Details = "Detail", Stock = "10", Price = "10 " };

            //Get the number of products in the database
            int count = await context.Product.CountAsync();


            //Act
            productController.Create(productViewModel);


            //Assert
            ////Verify if Product.Count has been incremented 
            Assert.Equal(count + 1, context.Product.Count());

            //Search if the product exists in the database
            var product = await context.Product.Where(x => x.Name == "toto").FirstOrDefaultAsync();
            Assert.NotNull(product);

            // Cleaning Database to reset the state
            context.Product.Remove(product);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task DeleteProduct()
        {
            //Arrange        
            var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer("Data Source=.;Initial Catalog=TestBase;Integrated Security=True").Options;

            P3Referential context = new(options, _configuration);
            LanguageService languageService = new();
            Cart cart = new();
            ProductRepository productRepository = new(context);
            OrderRepository orderRepository = new(context);
            ProductService productService = new(cart, productRepository, orderRepository, _localizer);
            ProductController productController = new(productService, languageService);
            ProductViewModel productViewModel = new() { Name = "titi", Description = "Description ", Details = "Detail", Stock = "10", Price = "10 " };

            //Get the number of products in the database
            int count = await context.Product.CountAsync();

            //Create a product to delete
            productController.Create(productViewModel);
            var product = await context.Product.Where(x => x.Name == "titi").FirstOrDefaultAsync();


            //Act
            //Delete the product
            try
            {
                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                productController.DeleteProduct(product.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            //Assert
            //Verify if Product.Count has been decremented 
            Assert.Equal(count, context.Product.Count());

            //Search the product in the database
            var productDontExistsAnymore = await context.Product.Where(p => p.Name == "titi").FirstOrDefaultAsync();


            //Verify if the Product has been deleted
            Assert.Null(productDontExistsAnymore);

        }

    }
}