using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace API.Helper
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string [] _extesions;
        public AllowedExtensionsAttribute(string[] extesions)
        {
            _extesions = extesions;
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension != null && !_extesions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"This file extension is not allowed!";
        }
    }
}