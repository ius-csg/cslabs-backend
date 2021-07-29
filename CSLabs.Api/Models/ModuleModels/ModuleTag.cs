using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models.Enums;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSLabs.Api.Models.ModuleModels
{
    public class ModuleTag
    {
        public int ModuleId { get; set; }
        [ForeignKey(nameof(ModuleId))]
        public Module Module { get; set; }
        
        public int TagId { get; set; }
        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; }
        
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<ModuleTag>()
                .HasKey(t => new { t.ModuleId, t.TagId });

            modelBuilder.Entity<ModuleTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(p => p.ModuleTags)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<ModuleTag>()
                .HasOne(pt => pt.Module)
                .WithMany(t => t.ModuleTags)
                .HasForeignKey(pt => pt.ModuleId);
        }
    }
}