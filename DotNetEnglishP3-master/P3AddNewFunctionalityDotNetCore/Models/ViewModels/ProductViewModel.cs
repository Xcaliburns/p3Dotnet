using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
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
        [RegularExpression(@"^\s*-?\d+\s*$", ErrorMessage = "StockNotAnInteger")]
        [Range(1, int.MaxValue, ErrorMessage = "StockNotGreaterThanZero")]
        public string Stock
        {
            get { return _stock; }
            set
            { // delete spaces before and after the number 
                _stock = value.Trim();
            }
        }
        private string _stock;


        //// verification order is important for some tests
        [Required(ErrorMessage = "MissingPrice")]
        [RegularExpression(@"^\s*-?\d+([.,]\d+)?\s*$", ErrorMessage = "PriceNotANumber")]
        [Range(0.01, double.MaxValue, ErrorMessage = "PriceNotGreaterThanZero")]
        public string Price
        {
            get { return _price; }
            set
            {
                // delete spaces before and after the number 
                _price = value.Trim();
            }
        }
        private string _price;



    }
}




