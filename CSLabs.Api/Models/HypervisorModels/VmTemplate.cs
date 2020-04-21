using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CSLabs.Api.Models.HypervisorModels
{
    [Table("vm_template")]
    public class VmTemplate : IQemu
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public bool IsCoreRouter { get; set; }
        
        [InverseProperty(nameof(HypervisorVmTemplate.VmTemplate))]
        [JsonIgnore]
        public List<HypervisorVmTemplate> HypervisorVmTemplates { get; set; } = new List<HypervisorVmTemplate>();
        
        public int? OwnerId  { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [JsonIgnore]
        public User Owner { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VmTemplate>()
                .HasOne(t => t.Owner)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        }
        
    }
}