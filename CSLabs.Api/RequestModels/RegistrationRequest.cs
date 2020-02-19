using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CSLabs.Api.Models;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.RequestModels
{
    public class RegistrationRequest
    {
 
        [Required]
        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [RegularExpression(@"^.+@.+\..+$", ErrorMessage = "This must be a valid email")]
        public string Email { get; set; }
        
        public int? GraduationYear { get; set; }
        
        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }
        
        public GenericErrorResponse Validate(DefaultContext dbContext)
        {
            if (!string.IsNullOrEmpty(Email) && dbContext.Users.Count(u => u.Email == Email) != 0)
            {
                return new GenericErrorResponse { Message = "The specified email is already in use"};
            }

            return null;
        }

        public bool IsValid(DefaultContext dbContext)
        {
            return Validate(dbContext) == null;
        }
    }
}