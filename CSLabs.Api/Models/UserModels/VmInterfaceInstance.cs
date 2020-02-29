using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models.UserModels
{
    public class VmInterfaceInstance
    {
        public int Id { get; set; }
        public int UserLabVmId  { get; set; }
        [ForeignKey(nameof(UserLabVmId))]
        public UserLabVm UserLabVm { get; set; }
        
        public int BridgeInstanceId  { get; set; }
        [ForeignKey(nameof(BridgeInstanceId))]
        public BridgeInstance BridgeInstance { get; set; }
        
        public int VmInterfaceTemplateId  { get; set; }
        [ForeignKey(nameof(VmInterfaceTemplateId))]
        public VmInterfaceTemplate Template { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VmInterfaceInstance>().HasIndex(i => i.UserLabVmId);
            
            modelBuilder.Entity<VmInterfaceInstance>()
                .HasOne(i => i.UserLabVm)
                .WithMany(v => v.InterfaceInstances)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<VmInterfaceInstance>()
                .HasOne(i => i.BridgeInstance)
                .WithMany(v => v.InterfaceInstances)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}