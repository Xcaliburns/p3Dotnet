using Xunit;
using System.ComponentModel.DataAnnotations;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels; // Assurez-vous d'avoir une référence au projet contenant le modèle
using System.Collections.Generic;

public class ProductViewModelTests
{

    [Fact]

    public void ExampleTest()
    {
        //Arrange
        //Act
        //Assert
    }

    [Theory]
    [InlineData(null, "MissingName")]
    [InlineData("", "MissingName")]
    [InlineData(" ","MissingName")]
    public void TestNameValidation(string name, string expectedErrorMessage)
    {
        //Arrange
        // Créer une instance du modèle
        var model = new ProductViewModel { Name = name };

        // Créer un contexte de validation
        var validationContext = new ValidationContext(model) { MemberName = "Name" };

        // Créer une liste pour stocker les résultats de la validation
        var validationResults = new List<ValidationResult>();

        //Act
        // Valider la propriété Name
        var result = Validator.TryValidateProperty(model.Name, validationContext, validationResults);
        
        //Assert
        // Vérifier que la validation a échoué
        Assert.False(result);

        // Vérifier que le message d'erreur est correct
        Assert.Equal(expectedErrorMessage, validationResults[0].ErrorMessage);
    }

    [Theory]
    [InlineData(null, "MissingQuantity")]
    [InlineData("", "MissingQuantity")]
    [InlineData(" ", "MissingQuantity")]
    [InlineData("abc", "StockNotAnInteger")]
    [InlineData("0", "StockNotGreaterThanZero")]
    
    public void TestStockValidation(string stock, string expectedErrorMessage)
    {
        //Arrange
        // Créer une instance du modèle
        var model = new ProductViewModel { Stock = stock };

        // Créer un contexte de validation
        var validationContext = new ValidationContext(model) { MemberName = "Stock" };

        // Créer une liste pour stocker les résultats de la validation
        var validationResults = new List<ValidationResult>();


        //Act
        // Valider la propriété Stock
        var result = Validator.TryValidateProperty(model.Stock, validationContext, validationResults);

        //Assert
        // Vérifier que la validation a échoué
        Assert.False(result);

        // Vérifier que le message d'erreur est correct
        Assert.Equal(expectedErrorMessage, validationResults[0].ErrorMessage);
    }

    [Theory]
    [InlineData(null, "MissingPrice")]
    [InlineData("", "MissingPrice")]
    [InlineData(" ", "MissingPrice")]
    [InlineData("abc", "PriceNotANumber")]
    [InlineData("0", "PriceNotGreaterThanZero")]
    
    public void TestPriceValidation(string price, string expectedErrorMessage)
    {
        // Arrange
        // Créer une instance du modèle
        var model = new ProductViewModel { Price = price };

        // Créer un contexte de validation
        var validationContext = new ValidationContext(model) { MemberName = "Price" };

        // Créer une liste pour stocker les résultats de la validation
        var validationResults = new List<ValidationResult>();

        // Act
        // Valider la propriété Stock
        var result = Validator.TryValidateProperty(model.Price, validationContext, validationResults);

        //Assert
        // Vérifier que la validation a échoué
        Assert.False(result);

        // Vérifier que le message d'erreur est correct
        Assert.Equal(expectedErrorMessage, validationResults[0].ErrorMessage);
    }
}
