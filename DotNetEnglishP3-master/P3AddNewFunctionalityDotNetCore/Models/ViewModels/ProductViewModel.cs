using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;


namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "MissingName")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        // verification order is important for some tests
        [Required(ErrorMessage = "MissingQuantity")]  
        [RegularExpression(@"^-?\d+$", ErrorMessage = "StockNotAnInteger")]
        [Range(1, int.MaxValue, ErrorMessage = "StockNotGreaterThanZero")]
       
        public string Stock { get; set; }

        // verification order is important for some tests
        [Required(ErrorMessage = "MissingPrice")]
        [RegularExpression(@"^-?\d+(\.\d+)?$", ErrorMessage = "PriceNotANumber")]
        // double.epsilon est la plus petite valeur au dessus de zero
        [Range(double.Epsilon, int.MaxValue, ErrorMessage = "PriceNotGreaterThanZero")]
        
        public string Price { get; set; }
    }

}
