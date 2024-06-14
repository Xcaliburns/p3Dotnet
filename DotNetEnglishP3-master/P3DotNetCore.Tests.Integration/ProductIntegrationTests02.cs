using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;


namespace P3DotNetCore.Tests.Integration
{
    public class ProductIntegrationTests02 : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IProductService _productService;

        public ProductIntegrationTests02(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            // Utilisez le service provider du factory pour obtenir une instance de IProductService
            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            _productService = serviceProvider.GetRequiredService<IProductService>();
        }

        [Fact]
        public void Create_ReturnsviewResult_WhenModelStateIsNotValid()
        {
            // Arrange

            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<P3Referential>();


            ICart cart = serviceProvider.GetRequiredService<ICart>();
            IProductRepository productRepository = serviceProvider.GetRequiredService<IProductRepository>();
            IOrderRepository orderRepository = serviceProvider.GetRequiredService<IOrderRepository>();
            IStringLocalizer localizer = serviceProvider.GetRequiredService<IStringLocalizer<ProductController>>();
            ILanguageService languageService = serviceProvider.GetRequiredService<ILanguageService>();


            var controller = new ProductController(_productService, languageService);


            var product = new ProductViewModel
            {
                // Je délare un produit qui ne correspond pas aux règles de validation
                Name = "",
                Price = "10",
                Stock = "10",
                Description = "Description 1",
                Details = "Details 1"
            };

            // Invalidation manuelle de l'état du modèle
            controller.ModelState.AddModelError("", "Error");

            // Act

            var result = controller.Create(product);

            // Assert

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(product, viewResult.Model);

        }

        [Fact]
        public async Task Create_AddProduct_WhenModelStateIsValid()
        {

            // Arrange

            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<P3Referential>();

            
            ICart cart = serviceProvider.GetRequiredService<ICart>();
            IProductRepository productRepository = serviceProvider.GetRequiredService<IProductRepository>();
            IOrderRepository orderRepository = serviceProvider.GetRequiredService<IOrderRepository>();
            IStringLocalizer localizer = serviceProvider.GetRequiredService<IStringLocalizer<ProductController>>();
            ILanguageService languageService = serviceProvider.GetRequiredService<ILanguageService>();


            var controller = new ProductController(_productService, languageService);


            var product = new ProductViewModel
            {
                // Je délare un produit qui  correspond  aux règles de validation
                Name = "toto",
                Price = "10",
                Stock = "10",
                Description = "Description 1",
                Details = "Details 1"
            };

            // Act

            controller.Create(product);
            //_productService.SaveProduct(product);

            // Assert

            // Vérifiez que le produit a été ajouté à la base de données
            var products = await productRepository.GetProduct();
            Assert.Contains(products, p => p.Name == product.Name);



        }

        [Fact]
        public async Task DeleteProduct_WhenModel()
        {

            // Arrange

            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<P3Referential>();


            ICart cart = serviceProvider.GetRequiredService<ICart>();
            IProductRepository productRepository = serviceProvider.GetRequiredService<IProductRepository>();
            IOrderRepository orderRepository = serviceProvider.GetRequiredService<IOrderRepository>();
            IStringLocalizer localizer = serviceProvider.GetRequiredService<IStringLocalizer<ProductController>>();

            ILanguageService languageService = serviceProvider.GetRequiredService<ILanguageService>();


            var controller = new ProductController(_productService, languageService);

            var productId = 53;
            // il faut s'assurer que le produit existe dans la base de données et ensuite l'effacer

            // Act

            controller.DeleteProduct(productId);

            // Assert

            // Vérifiez que le produit a été retiré de la base de données
            var products = await productRepository.GetProduct();
            Assert.DoesNotContain(products, p => p.Id == productId);



        }
    }
}
