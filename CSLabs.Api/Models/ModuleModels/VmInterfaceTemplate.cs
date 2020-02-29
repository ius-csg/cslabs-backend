using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models.ModuleModels
{
    public class VmInterfaceTemplate : IVmInterface
    {
        public int Id { get; set; }
        public int InterfaceNumber { get; set; }
        
        public int LabVmId  { get; set; }
        [ForeignKey(nameof(LabVmId))]
        public LabVm LabVm { get; set; }
        
        public int HypervisorBridgeTemplateId  { get; set; }
        [ForeignKey(nameof(HypervisorBridgeTemplateId))]
        public BridgeTemplate BridgeTemplate { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VmInterfaceTemplate>().HasIndex(t => t.LabVmId);
            modelBuilder.Unique<VmInterfaceTemplate>(t => new {t.InterfaceNumber, t.LabVmId});
        }
    }
}