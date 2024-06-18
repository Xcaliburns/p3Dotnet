using Xunit;
// ne pas oublier la référence au projet contenant le modèle
using P3AddNewFunctionalityDotNetCore.Models.ViewModels; 
using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models;



namespace P3AddNewFunctionalityDotNetCore;
public class ProductViewModelTests
{

    private Mock<ICart> mockCart;
    private Mock<IOrderRepository> mockOrder;
    private Mock<IProductRepository> mockProductRepository;
    private Mock<IStringLocalizer<ProductService>> mockStringLocalizer;
    private ProductService productService;

    public ProductViewModelTests()
    {
        mockCart = new Mock<ICart>();
        mockOrder = new Mock<IOrderRepository>();
        mockProductRepository = new Mock<IProductRepository>();
        mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();
        productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);
    }


    [Fact]
    public void When_PriceIsMissing_Returns_ErrorMessage()
    {
        //Arrange      

        var error = new LocalizedString("MissingPrice", "Price is missing");
        mockStringLocalizer.Setup(ml => ml["MissingPrice"]).Returns(error);

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        var result = productService.CheckProductModelErrors(new ProductViewModel { Name = "Product", Price = "", Description = "Test", Stock = "1", Details = "test" });

        //Assert

        Assert.Contains("Price is missing", result);
    }

    [Fact]
    public void When_PriceIsIsNotANumber_Returns_ErrorMessage()
    {
        //Arrange
        
        var error = new LocalizedString("PriceNotANumber", "Price is not number");
        mockStringLocalizer.Setup(ml => ml["PriceNotANumber"]).Returns(error);

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        var result = productService.CheckProductModelErrors(new ProductViewModel { Name = "Product", Price = "toto", Description = "Test", Stock = "1", Details = "test" });

        //Assert

        Assert.Contains("Price is not an integer", result);
    }

    [Fact]
    public void When_PriceIsIsNotGreaterThanZero_Returns_ErrorMessage()
    {
        //Arrange
        
        var error = new LocalizedString("PriceNotGreaterThanZero", "The price must be greater than zero");
        mockStringLocalizer.Setup(ml => ml["PriceNotGreaterThanZero"]).Returns(error);

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        var result = productService.CheckProductModelErrors(new ProductViewModel { Name = "Product", Price = "0", Description = "Test", Stock = "1", Details = "test" });

        //Assert

        Assert.Contains("The price must be greater than zero", result);
    }

    [Fact]
    public void When_MissingName_Returns_ErrorMessage()
    {
        //Arrange
       
        var error = new LocalizedString("MissingName", "Please enter a name");
        mockStringLocalizer.Setup(ml => ml["MissingName"]).Returns(error);

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        var result = productService.CheckProductModelErrors(new ProductViewModel { Name = "", Price = "2", Description = "Test", Stock = "1", Details = "test" });

        //Assert

        Assert.Contains("Please enter a name", result);
    }

    [Fact]
    public void When_MissingQuantity_Returns_ErrorMessage()
    {
        //Arrange
       
        var error = new LocalizedString("MissingQuantity", "Please enter a stock value");
        mockStringLocalizer.Setup(ml => ml["MissingQuantity"]).Returns(error);

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        var result = productService.CheckProductModelErrors(new ProductViewModel { Name = "toto", Price = "2", Description = "Test", Stock = "", Details = "test" });

        //Assert

        Assert.Contains("Please enter a stock value", result);
    }

    [Fact]
    public void When_StockNotAnInteger_Returns_ErrorMessage()
    {
        //Arrange
       
        var error = new LocalizedString("StockNotAnInteger", "The value entered for the stock must be a integer");
        mockStringLocalizer.Setup(ml => ml["StockNotAnInteger"]).Returns(error);

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        var result = productService.CheckProductModelErrors(new ProductViewModel { Name = "toto", Price = "2", Description = "Test", Stock = "titi", Details = "test" });

        //Assert

        Assert.Contains("The value entered for the stock must be a integer", result);
    }

    [Fact]
    public void When_StockNotGreaterThanZero_Returns_ErrorMessage()
    {
        //Arrange
      
        var error = new LocalizedString("StockNotGreaterThanZero", "The stock must greater than zero");
        mockStringLocalizer.Setup(ml => ml["StockNotGreaterThanZero"]).Returns(error);

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        var result = productService.CheckProductModelErrors(new ProductViewModel { Name = "toto", Price = "2", Description = "Test", Stock = "0", Details = "test" });

        //Assert

        Assert.Contains("The stock must greater than zero", result);
    }

    [Fact]
    public void When_ModelIsValid_Returns_EmptyList()
    {
        //Arrange
       
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        var result = productService.CheckProductModelErrors(new ProductViewModel { Name = "toto", Price = "2.22", Description = "Test", Stock = "2", Details = "test" });

        //Assert
        Assert.True(result.Count==0);
    }

}
