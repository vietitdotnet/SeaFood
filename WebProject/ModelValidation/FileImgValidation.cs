using System.ComponentModel.DataAnnotations;

namespace WebProject.ModelValidation
{
    public class FileImgValidation : ValidationAttribute
    {
        private readonly string[] _extensions;

        public FileImgValidation(string[] extensions)
        {
            _extensions = extensions;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string errorType;
            if (!IsValidAllowedExtensions(value))
            {
                errorType = "đuôi mở rộng phải là (.jpg .jpeg .png .jfif .webp) !";
            }
            else
            {
                return ValidationResult.Success;
            }

            ErrorMessage = $"{validationContext.DisplayName} dữ liệu  {errorType}";

            return new ValidationResult(ErrorMessage);
        }

        bool IsValidAllowedExtensions(object value)
        {
            var file = value as IFormFile[];
            if (file != null)
            {
                foreach (var item in file)
                {

                    var extension = Path.GetExtension(item.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return false;
                    }

                }
            }

            return true;
        }
    }
}