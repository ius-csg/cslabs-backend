using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CSLabs.Api.Models.ModuleModels
{
    public class BridgeTemplate
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public bool IsCoreBridge { get; set; }
        
        // a id generated on the frontend to link template bridges and interfaces
        [Required]
        public string Uuid { get; set; }
        public int LabId  { get; set; }
        [ForeignKey(nameof(LabId))]
        [JsonIgnore]
        public Lab Lab { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BridgeTemplate>().HasIndex(t => t.LabId);
            modelBuilder.Unique<BridgeTemplate>(t => new {t.LabId, t.Name});
            modelBuilder.Unique<BridgeTemplate>(t => new {t.LabId, t.Uuid});
        }
        
    }
}