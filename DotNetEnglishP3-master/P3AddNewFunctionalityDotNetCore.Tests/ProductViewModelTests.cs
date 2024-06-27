using Xunit;
// ne pas oublier la référence au projet contenant le modèle
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models;
using System.Collections.Generic;
using System.Globalization;
// added reference to the extension project
using P3AddNewFunctionalityDotNetCore.Extensions;



namespace P3AddNewFunctionalityDotNetCore;
public class ProductViewModelTests
{

    private Mock<ICart> mockCart;
    private Mock<IOrderRepository> mockOrder;
    private Mock<IProductRepository> mockProductRepository;
    private Mock<IStringLocalizer<ProductService>> mockStringLocalizer;
    private ProductService productService;

    // Déclaration du dictionnaire d'erreurs ici
    private Dictionary<string, string> errorsEnglish = new Dictionary<string, string>
    {
        { "MissingPrice", "Price is missing" },
        { "MissingQuantity", "Quantity is missing" },
        { "MissingName", "Name is missing" },
        { "PriceNotANumber", "Price is not a number" },
        { "StockNotAnInteger", "Stock is not an integer" },
        { "StockNotGreaterThanZero", "Stock must be greater than zero" },
        { "PriceNotGreaterThanZero", "Price must be greater than zero" }

    };
    private Dictionary<string, string> errorsFrench = new Dictionary<string, string>
    {
        { "MissingPrice", "le prix est manquant" },
        { "MissingQuantity", "le stock est manquant" },
        { "MissingName", "le nom est manquant" },
        { "PriceNotANumber", "le prix n'est pas un nombre" },
        { "StockNotAnInteger", "le stock n'est pas un entier" },
        { "StockNotGreaterThanZero", "le stock doit etre un nombre positif" },
        { "PriceNotGreaterThanZero", "le prix doit etre un nombre positif" }

    };

    public ProductViewModelTests()
    {
        // mocks initialisation to create a fake cart, order, productRepository and stringLocalizer an a new instance of ProductService
        mockCart = new Mock<ICart>();
        mockOrder = new Mock<IOrderRepository>();
        mockProductRepository = new Mock<IProductRepository>();
        mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();
        productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);
        // Configuration du mockStringLocalizer ici
        foreach (var error in errorsEnglish)
        {
            var localizedString = new LocalizedString(error.Key, error.Value);
            mockStringLocalizer.Setup(ml => ml[error.Key]).Returns(localizedString);
        }

        foreach (var error in errorsFrench)
        {
            var localizedString = new LocalizedString(error.Key, error.Value);
            mockStringLocalizer.Setup(ml => ml[error.Key]).Returns(localizedString);
        }
    }



    [Theory]
    [InlineData("fr", "toto", "", "Test", "titi", "test")]
    [InlineData("en", "toto", "", "Test", "titi", "test")]
    [InlineData("fr", "toto", "    ", "Test", "titi", "test")]
    [InlineData("en", "toto", "    ", "Test", "titi", "test")]
    public void When_PriceIsMissing_Returns_ErrorMessage(string culture, string name, string price, string description, string stock, string details)
    {
        //Arrange
        var product = new ProductViewModel { Name = name, Price = price, Description = description, Stock = stock, Details = details };

        //Act
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo(culture))["MissingPrice"].Value, result);
    }



    [Theory]
    [InlineData("fr", "Product", "toto", "Test", "1", "test")]
    [InlineData("en", "Product", "toto", "Test", "1", "test")]

    public void When_PriceIsNotANumber_Returns_ErrorMessage(string culture, string name, string price, string description, string stock, string details)
    {
        // Arrange
        var product = new ProductViewModel { Name = name, Price = price, Description = description, Stock = stock, Details = details };

        // Act
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        // Assert
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo(culture))["PriceNotANumber"].Value, result);
    }


    [Theory]
    [InlineData("fr", "Product", "0", "Test", "1", "test")]
    [InlineData("en", "Product", "0", "Test", "1", "test")]
    public void When_PriceIsNotGreaterThanZero_Returns_ErrorMessage(string culture, string name, string price, string description, string stock, string details)
    {
        //Arrange
        var product = new ProductViewModel { Name = name, Price = price, Description = description, Stock = stock, Details = details };
        //Act
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo(culture))["PriceNotGreaterThanZero"].Value, result);

    }


    [Theory]
    [InlineData("fr", "", "2.22", "Test", "1", "test")]
    [InlineData("en", "", "2", "Test", "1", "test")]
    [InlineData("fr", "   ", "2", "Test", "1", "test")]
    [InlineData("en", "   ", "2", "Test", "1", "test")]
    public void When_MissingName_Returns_ErrorMessage(string culture, string name, string price, string description, string stock, string details)
    {
        //Arrange
        var product = new ProductViewModel { Name = name, Price = price, Description = description, Stock = stock, Details = details };


        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        // creation of a product with a missing name
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the error message is returned
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo(culture))["MissingName"].Value, result);

    }

    [Theory]
    [InlineData("fr", "toto", "2", "Test", "", "test")]
    [InlineData("en", "toto", "2", "Test", "", "test")]
    [InlineData("fr", "toto", "2", "Test", "   ", "test")]
    [InlineData("en", "toto", "2", "Test", "   ", "test")]
    public void When_MissingQuantity_Returns_ErrorMessage(string culture, string name, string price, string description, string stock, string details)
    {
        //Arrange
        var product = new ProductViewModel { Name = name, Price = price, Description = description, Stock = stock, Details = details };

        // creation of a new ProductService instance
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        //creation of a product with a  missing stock
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the error message is returned
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo(culture))["MissingQuantity"].Value, result);

    }


    [Theory]
    [InlineData("en", "toto", "2.22", "Test", "titi", "test")]
    [InlineData("fr", "toto", "2.22", "Test", "titi", "test")]
    public void When_StockNotAnInteger_Returns_ErrorMessage(string culture, string name, string price, string description, string stock, string details)
    {
        //Arrange
        var product = new ProductViewModel { Name = name, Price = price, Description = description, Stock = stock, Details = details };

        // creation of a new ProductService instance
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        //creation of a product with a stock that is not an integer
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the error message is returned
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo(culture))["StockNotAnInteger"].Value, result);

    }

    [Theory]
    [InlineData("en", "toto", "2.22", "Test", "0", "test")]
    [InlineData("fr", "toto", "2.22", "Test", "0", "test")]
    public void When_StockNotGreaterThanZero_Returns_ErrorMessage(string culture, string name, string price, string description, string stock, string details)
    {
        //Arrange
        var product = new ProductViewModel { Name = name, Price = price, Description = description, Stock = stock, Details = details };


        // creation of a new ProductService instance
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        //creation of a product with a stock that is not greater than zero
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the error message is returned
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo(culture))["StockNotGreaterThanZero"].Value, result);

    }

    [Theory]
    [InlineData("toto", "2.22", "Test", "2", "test")]
    [InlineData("tata", "3.33", "Test2", "  3  ", "test2")]
    public void When_ModelIsValid_Returns_EmptyList(string name, string price, string description, string stock, string details)
    {
        //Arrange
        var product = new ProductViewModel { Name = name, Price = price, Description = description, Stock = stock, Details = details };
        // creation of a new ProductService instance
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        //creation of a valid product
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the list is empty
        Assert.True(result.Count == 0);
    }



}
