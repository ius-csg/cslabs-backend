using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        
        [Column(TypeName = "VARCHAR(45)")]
        public string Email { get; set; }
        [Column(TypeName = "VARCHAR(100)")]
        public string EmailVerificationCode { get; set; }
        
        public int? GraduationYear { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public EUserRole Role { get; set; } = EUserRole.Guest;

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
        
        public bool CanEditModules()
        {
            return Role == EUserRole.Creator || Role == EUserRole.Admin;
        }

        public bool IsAdmin()
        {
            return Role == EUserRole.Admin;
        }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<User>();
            builder.Unique<User>(u => u.Email);
            builder.Unique<User>(u => u.CardCodeHash);
            builder.Unique<User>(u => u.PasswordRecoveryCode);
            builder.Entity<User>()
                .Property(b => b.Password)
                .HasDefaultValue(null);
            builder.Entity<User>().Property(p => p.Role).HasConversion<string>();
        }
    }
}
