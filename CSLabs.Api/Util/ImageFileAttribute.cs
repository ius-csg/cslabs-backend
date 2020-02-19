using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace CSLabs.Api.Util
{
    public class ImageFileAttribute : ValidationAttribute
    {
        private readonly List<string> _extensions = IFormFileExtensions.AcceptedImageExtensions;

        public int MaxFileSizeMB { get; set; } = 10;
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var maxFileSize = MaxFileSizeMB * 1024 * 1024;
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                    return new ValidationResult(GetErrorMessage());
                if (!file.IsImage())
                    return new ValidationResult(GetInvalidImageMessage());
                if (file.Length > maxFileSize)
                    return new ValidationResult(GetSizeErrorMessage());
                
            } else if (value is List<IFormFile> files) {
                foreach (var formFile in files)
                {
                  
                    var extension = Path.GetExtension(formFile.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                        return new ValidationResult(GetErrorMessage());
                    if (!formFile.IsImage()) 
                        return new ValidationResult(GetInvalidImageMessage());
                    if (formFile.Length > maxFileSize)
                        return new ValidationResult(GetSizeErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Only the following extensions are allowed:  " + string.Join(", " ,_extensions);
        } 
        
        public string GetInvalidImageMessage()
        {
            return $"Corrupt or invalid image uploaded";
        }

        public string GetSizeErrorMessage()
        {
            return $"Images cannot be larger than {MaxFileSizeMB} MB";
        }
    }
}