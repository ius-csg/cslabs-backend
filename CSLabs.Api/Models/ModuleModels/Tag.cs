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