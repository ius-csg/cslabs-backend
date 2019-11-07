using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models.ModuleModels
{
    public class Module : Trackable
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        [Required]
        public bool Published { get; set; }
        
        public string SpecialCode { get; set; }
        public List<Lab> Labs { get; set; }
        
        [NotMapped]
        public int UserModuleId { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<Module>();
            builder.Unique<Module>(u => u.Name);
            builder.Unique<Module>(u => u.SpecialCode);
        }
    }
}