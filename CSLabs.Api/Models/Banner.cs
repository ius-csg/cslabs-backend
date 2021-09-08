using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using CSLabs.Api.Models.Enums;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;


namespace CSLabs.Api.Models

{
    public class Banner : Trackable
    {

        public int Id { get; set; }
        
        [Required] 
        [JsonConverter(typeof(StringEnumConverter))]
        public EBannerType Type { get; set; } = EBannerType.Warning;
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public DateTime StartTime { get; set; }
        
       [Required]
        public DateTime EndTime { get; set; }
       
        public static void OnAlertCreation(ModelBuilder builder)
        {
            builder.Entity<Banner>().Property(p => p.Type).HasConversion<string>(); 
        }
       
    }
}