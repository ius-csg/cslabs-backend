using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSLabsBackend.Models.ModuleModels;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models.UserModels
{
    public class UserModule : Trackable
    {
        public int Id { get; set; }
        [Required]
        public int  UserId { get; set; }
        [Required]
        public int  ModuleId { get; set; }
        
        public Module Module { get; set; }
        public User User { get; set; }

        public List<UserLab> UserLabs { get; set; }
        
        public DateTime TerminationDate { get; set; }
        
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<UserModule>();
            builder.Entity<UserModule>().HasIndex(u => new {u.UserId, u.ModuleId}).IsUnique();
        }
    }
}