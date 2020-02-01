using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        
        [NotMapped]
        public string Token { get; set; }
        
        [Column(TypeName = "VARCHAR(100)")]
        public string SchoolEmail { get; set; }
        
        [Column(TypeName = "VARCHAR(45)")]
        public string PersonalEmail { get; set; }
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
        
        // many to many link
        public List<UserUserModule> UserUserModules { get; set; }

        public void SetNulls()
        {
            if (PersonalEmail.Length == 0) {
                PersonalEmail = null;
            }
            if (SchoolEmail.Length == 0) {
                SchoolEmail = null;
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
