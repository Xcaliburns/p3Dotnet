﻿
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
    public class IntegrationTests
    {   //Arrange
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
            int count = await context.Product.CountAsync();


            //Act
            productController.Create(productViewModel);


            //Assert
            ////Product Count has been upped by 1
            Assert.Equal(count + 1, context.Product.Count());
            //Search the product in the database
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
            .UseSqlServer("Data Source=.;Initial Catalog=TestBase;Integrated Security=True").Options;

            P3Referential context = new(options, _configuration);
            LanguageService languageService = new();
            Cart cart = new();
            ProductRepository productRepository = new(context);
            OrderRepository orderRepository = new(context);
            ProductService productService = new(cart, productRepository, orderRepository, _localizer);
            ProductController productController = new(productService, languageService);            
            ProductViewModel productViewModel = new() { Name = "titi", Description = "Description ", Details = "Detail", Stock = "10", Price = "10 " };
            int count = await context.Product.CountAsync();

            //Create a product
            productController.Create(productViewModel);
            var product = await context.Product.Where(x => x.Name == "titi").FirstOrDefaultAsync();


            //Act
            //Delete the product
            productController.DeleteProduct(product.Id);

            //Assert
            //Verify if Product Count has been decreased by 1
            Assert.Equal(count, context.Product.Count());
            //Search the product in the database
            var productDontExistsAnymore = await context.Product.Where(x => x.Name == "titi").FirstOrDefaultAsync();
            //Verify if the Product has been deleted
            Assert.Null(productDontExistsAnymore);

        }

    }
}