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


namespace P3DotNetCore.Tests.Integration
{
	public class ProductIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
	{
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program>
            _factory;

        public ProductIntegrationTests()
		{
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }


		[Fact]
		public void Create_ReturnsviewResult_WhenModelStateIsNotValid()
		{

            // Arrange

            using var scope = _factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<P3Referential>();

           
           
            ICart cart = new Cart();
            IProductRepository productRepository = new ProductRepository(context);
            IOrderRepository orderRepository = new OrderRepository(context);
            IStringLocalizer localizer = serviceProvider.GetRequiredService<IStringLocalizer<ProductController>>();
            IProductService IProductService = new ProductService(cart, productRepository, orderRepository, localizer);
            ILanguageService ILanguageService = new LanguageService();


            var controller = new ProductController(IProductService,ILanguageService);


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
	}
}
