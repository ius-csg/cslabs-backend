using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CSLabs.Api.Models.ModuleModels
{
    public class VmInterfaceTemplate : IVmInterface
    {
        public int Id { get; set; }
        public int InterfaceNumber { get; set; }
        
        public int LabVmId  { get; set; }
        [ForeignKey(nameof(LabVmId))]
        [JsonIgnore]
        public LabVm LabVm { get; set; }
        // used to link the bridge on the frontend so the references can be linked on the backend.
        [Required]
        public string BridgeTemplateUuid { get; set; }
        public int BridgeTemplateId  { get; set; }
        [ForeignKey(nameof(BridgeTemplateId))]
        [JsonIgnore]
        public BridgeTemplate BridgeTemplate { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VmInterfaceTemplate>().HasIndex(t => t.LabVmId);
            modelBuilder.Unique<VmInterfaceTemplate>(t => new {t.InterfaceNumber, t.LabVmId});
        }
    }
}