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

        [Required(ErrorMessage = "MissingQuantity")]
        [Range(1, int.MaxValue, ErrorMessage = "QuantityNotGreaterThanZero")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "QuantityNotAnInteger")]
        public string Stock { get; set; }

        [Required(ErrorMessage = "MissingPrice")]
        [Range(1, int.MaxValue, ErrorMessage = "PriceNotGreaterThanZero")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "PriceNotANumber")]
        public string Price { get; set; }
    }
}
