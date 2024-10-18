using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebProject.ModelValidation
{
    public class PhoneValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string errorType;
            if (!IsValidPhone(value))
            {
                errorType = "Số điện thoại không hợp lệ";
            }
            else
            {
                return ValidationResult.Success;
            }

            ErrorMessage = $"{errorType}";

            return new ValidationResult(ErrorMessage);
        }

        bool IsValidPhone(object value)
        {
            string regex = @"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$";
            if (value != null)
                return Regex.IsMatch(value.ToString(), regex);

            else return false;
        }
    }
}
