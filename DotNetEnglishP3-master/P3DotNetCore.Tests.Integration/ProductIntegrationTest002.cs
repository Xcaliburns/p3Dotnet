
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using NuGet.ContentModel;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;


namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class IntegrationTests
    {   //Arrange
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<ProductService> _localizer;
        [Fact]
        public async Task SaveNewProduct()
        {
            //Arrange        
            var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer("Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            P3Referential context = new(options, _configuration);
            LanguageService languageService = new();
            Cart cart = new();
            ProductRepository productRepository = new(context);
            OrderRepository orderRepository = new(context);
            ProductService productService = new(cart, productRepository, orderRepository, _localizer);
            ProductController productController = new(productService, languageService);
            ProductViewModel productViewModel = new() { Name = "toto", Description = "Description ", Details = "Detail", Stock = "10", Price = "10 " };
            int count = await context.Product.CountAsync();


            //Act
            productController.Create(productViewModel);


            //Assert
            ////Product Count has been upped by 1
            Assert.Equal(count + 1, context.Product.Count());
            ////Product can be found
            var product = await context.Product.Where(x => x.Name == "toto").FirstOrDefaultAsync();
            Assert.NotNull(product);
            // Cleaning Database
            context.Product.Remove(product);
            await context.SaveChangesAsync();
        }
        [Fact]
        public async Task DeleteProduct()
        {
            //Arrange        
            var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer("Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            P3Referential context = new(options, _configuration);
            LanguageService languageService = new();
            Cart cart = new();
            ProductRepository productRepository = new(context);
            OrderRepository orderRepository = new(context);
            ProductService productService = new(cart, productRepository, orderRepository, _localizer);
            ProductController productController = new(productService, languageService);
            ProductViewModel productViewModel = new() { Name = "titi", Description = "Description ", Details = "Detail", Stock = "10", Price = "10 " };
            int count = await context.Product.CountAsync();
            productController.Create(productViewModel);
            var product = await context.Product.Where(x => x.Name == "titi").FirstOrDefaultAsync();


            //Act
            productController.DeleteProduct(product.Id);

            //Assert
            Assert.Equal(count, context.Product.Count());
            var searchProductAgain = await context.Product.Where(x => x.Name == "titi").FirstOrDefaultAsync();
            Assert.Null(searchProductAgain);

        }

    }
}