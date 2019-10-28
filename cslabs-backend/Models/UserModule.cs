using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models
{
    public class UserModule
    {
        public int id { get; set; }
        public int  UserId { get; set; }
        public int  ModuleId { get; set; }
        public DateTime TerminateDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public virtual ICollection<Module > Module { get; set; }
        public virtual User User { get; set; }
        
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<Module>();
        }
    }
}