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
    public class Module : Trackable
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public bool Published { get; set; }
        
        public string SpecialCode { get; set; }
        public List<Lab> Labs { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public EModuleType Type { get; set; }
        
        [NotMapped]
        public int UserModuleId { get; set; }

        public async Task SetUserModuleIdIfExists(DefaultContext context, User user)
        {
            var userModule = await context.UserModules
                .WhereIncludesUser(user)
                .Where(um => um.ModuleId == Id)
                .FirstOrDefaultAsync();
            if (userModule != null)
                UserModuleId = userModule.Id;
        }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<Module>();
            builder.Unique<Module>(u => u.Name);
            builder.Unique<Module>(u => u.SpecialCode);
            builder.Entity<Module>()
                .Property(p => p.Type)
                .HasConversion<string>()
                .HasDefaultValue(EModuleType.SingleUser);
        }
    }
}