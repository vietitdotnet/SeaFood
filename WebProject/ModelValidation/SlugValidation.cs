using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebProject.ModelValidation
{
    public class SlugValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string errorType;

            if (!IsValidSlug(value))
            {
                errorType = "không chứa khoảng trắng , không chứa kí tự đặt biệt, không dấu, kí tự viết hoa";
            }

            else
            {
                return ValidationResult.Success;
            }

            ErrorMessage = $"{validationContext.DisplayName} {errorType}";

            return new ValidationResult(ErrorMessage);
        }

        bool IsValidSlug(object value)
        {
            if (value != null)
            {
                var c = Regex.IsMatch(value.ToString(), @"^[a-z0-9-]*$");

                if (c)
                {
                    return true;
                }

                return false;
            }

            return true;
        }

    }
}
