using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models.UserModels
{
    public class BridgeInstance
    {
        public int Id { get; set; }
        [Required]
        // 0-4 is reserved, 5 and higher is used for the application.
        // references back to the actual bridge id in the hypervisor ex. proxmox
        public int HypervisorInterfaceId {get;set;}
        public int BridgeTemplateId  { get; set; }
        [ForeignKey(nameof(BridgeTemplateId))]
        public BridgeTemplate BridgeTemplate { get; set; }
        
        public int UserLabId  { get; set; }
        [ForeignKey(nameof(UserLabId))]
        public UserLab UserLab { get; set; }

        [InverseProperty(nameof(VmInterfaceInstance.BridgeInstance))]
        public List<VmInterfaceInstance> InterfaceInstances { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BridgeInstance>().HasIndex(i => i.UserLabId);
            modelBuilder.Unique<BridgeInstance>(i => new {i.UserLabId, InterfaceId = i.HypervisorInterfaceId});
        }
    }
}