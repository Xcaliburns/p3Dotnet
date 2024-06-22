using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static P3AddNewFunctionalityDotNetCore.Models.ViewModels.PositiveNumberAttribute;


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
        private string stock;
        [Required(ErrorMessage = "MissingQuantity")]
        // [TrimmedIntegerAttribute(ErrorMessage = "StockNotAnInteger")]
        [RegularExpression(@"^-?\d+$", ErrorMessage = "StockNotAnInteger")]
        [Range(1, int.MaxValue, ErrorMessage = "StockNotGreaterThanZero")]
        public string Stock
        {
            get { return stock; }
            set { stock = value.Trim(); }
        }

        // verification order is important for some tests
        [Required(ErrorMessage = "MissingPrice")]
        // [RegularExpression(@"^-?\d+([.,]\d{1,2})?$", ErrorMessage = "PriceNotANumber")]
        [DoubleFormatAttribute(ErrorMessage = "PriceNotANumber")]
        // double.epsilon est la plus petite valeur au dessus de zero
        [PositiveNumber(ErrorMessage = "PriceNotGreaterThanZero")]

        public string Price { get; set; }
    }

    public class DoubleFormatAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue)
            // Remove spaces before and after the number
            {
                stringValue = stringValue.Trim();

                if (!double.TryParse(stringValue.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }

    public class PositiveNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {


            if (value is string stringValue)
            {
                stringValue = stringValue.Replace(',', '.');
                if (double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double doubleValue))
                {
                    //return formated value
                    return doubleValue > 0;
                }
            }
            return false;
        }


        public class TrimmedIntegerAttribute : RegularExpressionAttribute
        {
            public TrimmedIntegerAttribute() : base(@"^-?\d+$")
            {
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is string stringValue)
                {
                    
                    // Use the base class's IsValid method to do the regex validation
                    return base.IsValid(stringValue, validationContext);
                }

                return ValidationResult.Success;
            }
        }

    }
}




