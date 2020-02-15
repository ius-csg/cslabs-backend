using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using CSLabs.Api.Models;
using CSLabs.Api.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.RequestModels
{
    public class ContactUsRequest
    {
        [RegularExpression(@"^.+@.+\..+$", ErrorMessage = "This must be a valid email")]
        public string Email { get; set; }

        public IFormFile UserScreenshot { get; set; }
        
        public string Body { get; set; } 
    }
}