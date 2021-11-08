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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSLabs.Api.Models.ModuleModels
{
    public class Module : Trackable, IPrimaryKeyModel
    {
        public int Id { get; set; }
        [Required]
        [UniqueInDB(ColumnName = "name")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public bool Published { get; set; }
        
        public string SpecialCode { get; set; }
        public List<Lab> Labs { get; set; }
        
        public List<ModuleTag> ModuleTags { get; set; }
        
        public int? OwnerId  { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [JsonIgnore]
        public User Owner { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public EModuleType Type { get; set; }
        
        [NotMapped]
        public int UserModuleId { get; set; }

        public async Task SetUserModuleIdIfExists(DefaultContext context, User user)
        {
            if (user == null)
                return;
            var userModule = await context.UserModules
                .WhereIncludesUser(user)
                .Where(um => um.ModuleId == Id)
                .FirstOrDefaultAsync();
            if (userModule != null)
                UserModuleId = userModule.Id;
        }

        public void AddModuleTags(DefaultContext context)
        {
            foreach (var moduleTag in ModuleTags.Where(moduleTag => moduleTag.TagId == 0))
            {
                context.Add(moduleTag);
                if (moduleTag.Tag.Id != 0)
                    context.Entry(moduleTag.Tag).State = EntityState.Unchanged;
            }
        }

        public void DeleteModuleTags(DefaultContext context)
        {
            var currentModuleTags = context.Modules.AsNoTracking().IncludeTags().First(m => m.Id == Id).ModuleTags;
            var tagsToBeDeleted = currentModuleTags.Where(mt => !ModuleTags.Contains(mt)).ToList();
            foreach (var moduleTag in tagsToBeDeleted)
            {
                context.Entry(moduleTag).State = EntityState.Deleted;
            }
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