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

    // Déclaration de votre dictionnaire d'erreurs ici
    private Dictionary<string, string> errorsEnglish = new Dictionary<string, string>
    {
        { "MissingPrice", "Price is missing" },
        { "MissingQuantity", "Quantity is missing" },
        { "MissingName", "Name is missing" },
        { "PriceNotANumber", "Price is not a number" },
        { "StockNotAnInteger", "Stock is not an integer" },
        { "StockNotGreaterThanZero", "Stock must be greater than zero" },
        { "PriceNotGreaterThanZero", "Price must be greater than zero" }
        // Ajoutez ici d'autres erreurs de validation que vous voulez tester
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
        // Ajoutez ici d'autres erreurs de validation que vous voulez tester
    };

    public ProductViewModelTests()
    {
        // mocks initialisation to create a fake cart, order, productRepository and stringLocalizer an a new instance of ProductService
        mockCart = new Mock<ICart>();
        mockOrder = new Mock<IOrderRepository>();
        mockProductRepository = new Mock<IProductRepository>();
        mockStringLocalizer = new Mock<IStringLocalizer<ProductService>>();
        productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);
        // Configuration de votre mockStringLocalizer ici
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




    [Fact]
    public void When_PriceIsMissing_Returns_ErrorMessage()
    {
        //Arrange
        var product = new ProductViewModel { Name = "Product", Price = "", Description = "Test", Stock = "1", Details = "test" };
        //Act
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo("en"))["MissingPrice"].Value, result);
    }
    [Fact]
    public void When_PriceIsBlank_Returns_ErrorMessage()
    {

        //Arrange
        var product = new ProductViewModel { Name = "Product", Price = "  ", Description = "Test", Stock = "1", Details = "test" };

        //Act
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo("en"))["MissingPrice"].Value, result);
    }

    [Fact]
    public void When_PriceIsNotANumber_Returns_ErrorMessage()
    {
        // Arrange
        var product = new ProductViewModel { Name = "Product", Price = "toto", Description = "Test", Stock = "1", Details = "test" };

        // Act
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        // Assert
        var expectedErrorMessage = mockStringLocalizer.Object.WithCulture(new CultureInfo("en"))["PriceNotANumber"].Value;
        Assert.Contains(expectedErrorMessage, result);
    }

    [Fact]
    public void When_PriceIsNotGreaterThanZero_Returns_ErrorMessage()
    {
        //Arrange
        var product = new ProductViewModel { Name = "Product", Price = "0", Description = "Test", Stock = "1", Details = "test" };
        //Act
        var result = productService.CheckProductModelErrors( product, mockStringLocalizer.Object);

        //Assert
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo("en"))["PriceNotGreaterThanZero"].Value, result);
    }


    [Fact]
    public void When_MissingName_Returns_ErrorMessage()
    {
        //Arrange
        var product = new ProductViewModel { Name = "", Price = "2", Description = "Test", Stock = "1", Details = "test" };
       

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        // creation of a product with a missing name
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the error message is returned
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo("en"))["MissingName"].Value, result);
    }

    [Fact]
    public void When_NameIsBlank_Returns_ErrorMessage()
    {
        //Arrange
        var product = new ProductViewModel { Name = "  ", Price = "2", Description = "Test", Stock = "1", Details = "test" };
        // error message to be returned when the name is blank
        

        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        // creation of a product with a blank name
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the error message is returned
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo("en"))["MissingName"].Value, result);
    }
    [Fact]
    public void When_MissingQuantity_Returns_ErrorMessage()
    {
        //Arrange
        var product = new ProductViewModel { Name = "toto", Price = "2", Description = "Test", Stock = "", Details = "test" };

        // creation of a new ProductService instance
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        //creation of a product with a  missing stock
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the error message is returned
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo("en"))["MissingQuantity"].Value, result);
    }

    [Fact]
    public void When_QuantityIsBlank_Returns_ErrorMessage()
    {
        //Arrange
        var product = new ProductViewModel { Name = "toto", Price = "2", Description = "Test", Stock = "   ", Details = "test" };
       
        // creation of a new ProductService instance
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        //creation of a product with a  blank stock
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the error message is returned
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo("en"))["MissingQuantity"].Value, result);
    }

    [Fact]
    public void When_StockNotAnInteger_Returns_ErrorMessage()
    {
        //Arrange
        var product = new ProductViewModel { Name = "toto", Price = "2", Description = "Test", Stock = "titi", Details = "test" };
               
        // creation of a new ProductService instance
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        //creation of a product with a stock that is not an integer
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the error message is returned
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo("en"))["StockNotAnInteger"].Value, result);
    }



    [Fact]
    public void When_StockNotGreaterThanZero_Returns_ErrorMessage()
    {
        //Arrange
        var product = new ProductViewModel { Name = "toto", Price = "2", Description = "Test", Stock = "0", Details = "test" };
       

        // creation of a new ProductService instance
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        //creation of a product with a stock that is not greater than zero
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the error message is returned
        Assert.Contains(mockStringLocalizer.Object.WithCulture(new CultureInfo("en"))["StockNotGreaterThanZero"].Value, result);
    }

    [Fact]
    public void When_ModelIsValid_Returns_EmptyList()
    {
        //Arrange
        var product = new ProductViewModel { Name = "toto", Price = "2.22", Description = "Test", Stock = "2", Details = "test" };
        // creation of a new ProductService instance
        var productService = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrder.Object, mockStringLocalizer.Object);

        //Act
        //creation of a valid product
        var result = productService.CheckProductModelErrors(product, mockStringLocalizer.Object);

        //Assert
        // check if the list is empty
        Assert.True(result.Count == 0);
    }

    [Fact]
    public void When_ModelIsValid_StockHasSpaces_Returns_EmptyList()
    {
        //Arrange
        var product = new ProductViewModel { Name = "toto", Price = "2.22", Description = "Test", Stock = "  2  ", Details = "test" };
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
