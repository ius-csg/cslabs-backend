using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models.UserModels
{
    public class UserModule : Trackable
    {
        public int Id { get; set; }
        
        [Required]
        public int  ModuleId { get; set; }
        
        public Module Module { get; set; }

        public List<UserLab> UserLabs { get; set; }
        
        public DateTime TerminationDate { get; set; }
        
        [InverseProperty(nameof(UserUserModule.UserModule))]
        public List<UserUserModule> UserUserModules { get; set; }
        
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<UserModule>();
        }
    }
}