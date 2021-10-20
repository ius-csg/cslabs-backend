using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using CSLabs.Api.Models;
using CSLabs.Api.Util;
using Zxcvbn;

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

            if (!ValidatePasswordStrength())
            {
                return new GenericErrorResponse { Message = "The provided password is not strong enough" };
            }

            return null;
        }

        public bool ValidatePasswordStrength()
        {
            return Zxcvbn.Core.EvaluatePassword(Password).Score >= 4;
        }
        
        public bool IsValid(DefaultContext dbContext)
        {
            return Validate(dbContext) == null;
        }
    }
}