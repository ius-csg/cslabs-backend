using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models
{
    public class Module : ITrackable
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        [Required]
        public bool Published { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Module>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("UTC_TIMESTAMP()");
            
            builder.Entity<Module>().HasIndex(u => u.Name).IsUnique();
            builder.Entity<Module>().HasIndex(u => u.ShortName).IsUnique();
        }
    }
}