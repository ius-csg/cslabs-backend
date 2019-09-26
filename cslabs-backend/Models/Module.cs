using System;
using System.ComponentModel.DataAnnotations;
using CSLabsBackend.Util;
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
            builder.TimeStamps<Module>();
            builder.Unique<Module>(u => u.Name);
            builder.Unique<Module>(u => u.ShortName);
        }
    }
}