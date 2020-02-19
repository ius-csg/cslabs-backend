
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSLabs.Api.Util;
using Microsoft.AspNetCore.Http;

namespace CSLabs.Api.RequestModels
{
    public class ContactUsRequest
    {
        [Required]
        [RegularExpression(@"^.+@.+\..+$", ErrorMessage = "This must be a valid email")]
        [MaxLength(100, ErrorMessage = "The email can at max be 100 characters long")]
        public string Email { get; set; }
        [ImageFile(MaxFileSizeMB = 3)]
        [MaxLength(10, ErrorMessage = "There cannot be more than 10 screenshots")]
        public List<IFormFile> Screenshots { get; set; } = new List<IFormFile>();
        [Required]
        [MaxLength(10_000, ErrorMessage = "The message can at max be 10k characters long")]
        public string Message { get; set; } 
    }
}