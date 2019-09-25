using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models
{
    public class User : ITrackable
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(45)")]
        public string FirstName { get; set; }

        [Column(TypeName = "VARCHAR(45)")]
        public string MiddleName { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(45)")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(45)")]
        public string SchoolEmail { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(45)")]
        public string PersonalEmail { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(45)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy}")]
        public DateTime GraduationYear { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(45)")]
        public userType UserType { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = "VARCHAR(45)")]
        public string CardCodeHash { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime TerminationDate { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("UTC_TIMESTAMP()");

            builder.Entity<User>().HasIndex(u => u.SchoolEmail).IsUnique();
            builder.Entity<User>().HasIndex(u => u.PersonalEmail).IsUnique();
        }
    }
}
