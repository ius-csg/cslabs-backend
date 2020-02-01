using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.Security;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static CSLabs.Api.Models.Enums.UserType;

namespace CSLabs.Api.Models.UserModels
{
    public class User : Trackable
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string FirstName { get; set; }
        
        [Column(TypeName = "VARCHAR(100)")]
        public string MiddleName { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Username { get; set; }
        
        [NotMapped]
        public string Token { get; set; }
        
        [Column(TypeName = "VARCHAR(100)")]
        public string SchoolEmail { get; set; }
        
        [Column(TypeName = "VARCHAR(45)")]
        public string PersonalEmail { get; set; }

        public string GetEmail()
        {
           return SchoolEmail ?? PersonalEmail;
        }

        [Column(TypeName = "VARCHAR(100)")]
        public string SchoolEmailVerificationCode { get; set; }
        
        [Column(TypeName = "VARCHAR(100)")]
        public string PersonalEmailVerificationCode { get; set; }
        
        public int? GraduationYear { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(45)")]
        public string UserType { get; set; } = Guest;

        [Column(TypeName = "VARCHAR(100)")]
        public string CardCodeHash { get; set; }
        
        [Required]
        public string Password { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime? TerminationDate { get; set; }
        
        [JsonIgnore]
        [Column(TypeName = "VARCHAR(100)")]
        public string PasswordRecoveryCode { get; set; }

        public void SetNulls()
        {
            if (PersonalEmail.Length == 0) {
                PersonalEmail = null;
            }
            if (SchoolEmail.Length == 0) {
                SchoolEmail = null;
            }
        }

        private string FillUsername()
        {
            string username;
            if (GetEmail().Contains("@iu.edu"))
            {
                return username = GetEmail().Remove(GetEmail().Length - 7);
            }
            else
            {
                return username = GetEmail().Trim(new Char[] {'.', '@'});
            }
        }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<User>();
            builder.Unique<User>(u => u.SchoolEmail);
            builder.Unique<User>(u => u.PersonalEmail);
            builder.Unique<User>(u => u.CardCodeHash);
            builder.Unique<User>(u => u.PasswordRecoveryCode);
            builder.Entity<User>()
                .Property(b => b.Password)
                .HasDefaultValue(null);
        }
    }
}
