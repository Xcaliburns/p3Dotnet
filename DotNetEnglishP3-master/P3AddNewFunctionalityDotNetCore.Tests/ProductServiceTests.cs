using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>
        [Fact]
        public void ExampleMethod()
        {
            // Arrange

            // Act


            // Assert
            Assert.Equal(1, 1);
        }

        // TODO write test methods to ensure a correct coverage of all possibilities
        [Fact]
        public void ProductName_RequiredAttribute_is_false_Test()
        {
            // Arrange
            var productViewModel = new ProductViewModel();
            var validationContext = new ValidationContext(productViewModel);
            var validationResults = new List<ValidationResult>();

            // Act
            // Laisser le nom vide pour tester l'attribut Required
            productViewModel.Name = "";
            productViewModel.Price = "5";
            productViewModel.Stock = "5";
            productViewModel.Details = "";
            productViewModel.Description = "";
            var isValid = Validator.TryValidateObject(productViewModel, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid); // Le modèle ne doit pas être valide car le nom est requis
            Assert.Contains(validationResults, r => r.ErrorMessage == "MissingName");
            // Le message d'erreur spécifique doit être présent

        }
        [Fact]
        public void ProductPrice_RequiredAttribute_is_false_Test()
        {
            // Arrange
            var productViewModel = new ProductViewModel();
            var validationContext = new ValidationContext(productViewModel);
            var validationResults = new List<ValidationResult>();

            // Act
            // Laisser le nom vide pour tester l'attribut Required
            productViewModel.Name = "toto";
            productViewModel.Price = "";
            productViewModel.Stock = "5";
            productViewModel.Details = "";
            productViewModel.Description = "";
            var isValid = Validator.TryValidateObject(productViewModel, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid); // Le modèle ne doit pas être valide car le prix est requis
            Assert.Contains(validationResults, r => r.ErrorMessage == "MissingPrice");
            // Le message d'erreur spécifique doit être présent

        }

        [Fact]
        public void ProductStock_RequiredAttribute_is_false_Test()
        {
            // Arrange
            var productViewModel = new ProductViewModel();
            var validationContext = new ValidationContext(productViewModel);
            var validationResults = new List<ValidationResult>();

            // Act
            // Laisser le nom vide pour tester l'attribut Required
            productViewModel.Name = "toto";
            productViewModel.Price = "5";
            productViewModel.Stock = "";
            productViewModel.Details = "";
            productViewModel.Description = "";
            var isValid = Validator.TryValidateObject(productViewModel, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid); // Le modèle ne doit pas être valide car le stock est requis
            Assert.Contains(validationResults, r => r.ErrorMessage == "MissingQuantity");
            // Le message d'erreur spécifique doit être présent

        }

        [Fact]
        public void ProductPrice_isANumberAttribute_is_false_Test()
        {
            // Arrange
            var productViewModel = new ProductViewModel();
            var validationContext = new ValidationContext(productViewModel);
            var validationResults = new List<ValidationResult>();

            // Act
            
            productViewModel.Name = "toto";
            productViewModel.Price = "f";
            productViewModel.Stock = "5";
            productViewModel.Details = "";
            productViewModel.Description = "";
            var isValid = Validator.TryValidateObject(productViewModel, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid); // Le modèle ne doit pas être valide car le prix doit etre un nombre
            Assert.Contains(validationResults, r => r.ErrorMessage == "PriceNotANumber");
            // Le message d'erreur spécifique doit être présent

        }

        [Fact]
        public void ProductStock_isANumberAttribute_is_false_Test()
        {
            // Arrange
            var productViewModel = new ProductViewModel();
            var validationContext = new ValidationContext(productViewModel);
            var validationResults = new List<ValidationResult>();

            // Act
            // Laisser le nom vide pour tester l'attribut Required
            productViewModel.Name = "toto";
            productViewModel.Price = "5";
            productViewModel.Stock = "f";
            productViewModel.Details = "";
            productViewModel.Description = "";
            var isValid = Validator.TryValidateObject(productViewModel, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid); // Le modèle ne doit pas être valide car le stock doit etre un nombre
            Assert.Contains(validationResults, r => r.ErrorMessage == "QuantityNotAnInteger");
            // Le message d'erreur spécifique doit être présent

        }
        [Fact]
        public void Product_is_valid_Test()
        {
            // Arrange
            var productViewModel = new ProductViewModel();
            var validationContext = new ValidationContext(productViewModel);
            var validationResults = new List<ValidationResult>();

            // Act
            
            productViewModel.Name = "toto";
            productViewModel.Price = "5";
            productViewModel.Stock = "5";
            productViewModel.Details = "";
            productViewModel.Description = "";
            var isValid = Validator.TryValidateObject(productViewModel, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid); // Le modèle  doit  être valide 
            
           

        }


    }
}