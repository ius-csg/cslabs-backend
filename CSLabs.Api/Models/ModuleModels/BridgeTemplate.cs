using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models.ModuleModels
{
    public class BridgeTemplate
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int LabId  { get; set; }
        [ForeignKey(nameof(LabId))]
        public Lab Lab { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BridgeTemplate>().HasIndex(t => t.LabId);
            modelBuilder.Unique<BridgeTemplate>(t => new {t.LabId, t.Name});
        }
        
    }
}