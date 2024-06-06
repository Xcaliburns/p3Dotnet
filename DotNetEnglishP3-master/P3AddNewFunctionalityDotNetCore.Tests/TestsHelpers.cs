using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    internal class TestsHelpers
    {
        public static Mock<ResourceManager> CreateMockResourceManager()
        {
            string errorMessage = "";
            var mockResourceManager = new Mock<ResourceManager>();
            mockResourceManager.Setup(rm => rm.GetString(errorMessage, It.IsAny<CultureInfo>()))
                               .Returns((string key, CultureInfo culture) =>
                               {

                                   if (errorMessage == "MissingName")
                                   {
                                       if (culture.Name == "fr-FR")
                                       {
                                           return "Veuillez entrer un nom";
                                       }
                                       else if (culture.Name == "en-EN")
                                       {
                                           return "Please enter a name";
                                       }
                                   }

                                   if (errorMessage == "MissingQuantity")
                                   {
                                       if (culture.Name == "fr-FR")
                                       {
                                           return "Veuillez saisir un stock";
                                       }
                                       else if (culture.Name == "en-EN")
                                       {
                                           return "Please enter a stock value";
                                       }
                                   }

                                   if (errorMessage == "MissingPrice")
                                   {
                                       if (culture.Name == "fr-FR")
                                       {
                                           return "Veuillez entrer un prix";
                                       }
                                       else if (culture.Name == "en-EN")
                                       {
                                           return "Please enter a price";
                                       }
                                   }
                                   if (errorMessage == "StockNotGreaterThanZero")
                                   {
                                       if (culture.Name == "fr-FR")
                                       {
                                           return "La stock doit être supérieure à zéro";
                                       }
                                       else if (culture.Name == "en-EN")
                                       {
                                           return "The stock must greater than zero";
                                       }
                                   }
                                   if (errorMessage == "StockNotAnInteger")
                                   {
                                       if (culture.Name == "fr-FR")
                                       {
                                           return "La valeur saisie pour le stock doit être un entier";
                                       }
                                       else if (culture.Name == "en-EN")
                                       {
                                           return "The value entered for the stock must be a integer";
                                       }
                                   }

                                   if (errorMessage == "PriceNotGreaterThanZero")
                                   {
                                       if (culture.Name == "fr-FR")
                                       {
                                           return "La prix doit être supérieur à zéro";
                                       }
                                       else if (culture.Name == "en-EN")
                                       {
                                           return "The price must be greater than zero";
                                       }
                                   }

                                   if (errorMessage == "PriceNotANumber")
                                   {
                                       if (culture.Name == "fr-FR")
                                       {
                                           return "La valeur saisie pour le prix doit être un nombre";
                                       }
                                       else if (culture.Name == "en-EN")
                                       {
                                           return "The value entered for the price must be a number";
                                       }
                                   }

                                   return null;
                               });
            return mockResourceManager;
        }
    }
}
