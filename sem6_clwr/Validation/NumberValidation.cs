using System;
using System.Globalization;
using System.Windows.Controls;

namespace sem6_clwr.Validation
{
    class NumberValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int i;
            if (!Int32.TryParse(value.ToString(), out i))
            {
                return new ValidationResult(false, "Please enter a valid number");
            }

            return new ValidationResult(true, null);
        }
    }
}
