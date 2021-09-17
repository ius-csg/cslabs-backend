using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CSLabs.Api.Models.Enums;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;


namespace CSLabs.Api.Models

{
    public class SystemMessage :  Trackable, IPrimaryKeyModel
    {
        public int Id { get; set; }

        [Required] 
        [JsonConverter(typeof(StringEnumConverter))]
        public ESystemMessageType Type { get; set; } = ESystemMessageType.Warning;
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
       [Required]
        public DateTime EndTime { get; set; }
       
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SystemMessage>().Property(p => p.Type).HasConversion<string>(); 
        }
       
    }
}