using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models
{
    public class Badge : Trackable
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "TEXT")]
        [Required]
        public string Description { get; set; }
        [Column(TypeName = "VARCHAR(255)")]
        [Required]
        public string IconPath { get; set; }
        
        public int RequiredNumOfModules { get; set; }
        
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<Badge>();
            builder.Unique<Badge>(u => u.Name);
        }
       
    }
}