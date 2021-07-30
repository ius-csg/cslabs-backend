using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CSLabs.Api.Models.ModuleModels
{
    public class Tag : IPrimaryKeyModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        [JsonIgnore]
        public List<ModuleTag> ModuleTags { get; set; }
        
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Unique<Tag>(t => t.Name);
        }
    }
}