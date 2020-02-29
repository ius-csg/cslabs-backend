using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CSLabs.Api.Models.UserModels
{
    public class UserLabVm : Trackable, IQemu
    {
        public int Id { get; set; }
        
        public int LabVmId { get; set; }
        [ForeignKey(nameof(LabVmId))]
        public LabVm LabVm { get; set; }
        
        [Required]
        public int ProxmoxVmId { get; set; }
        
        public bool IsCoreRouter { get; set; }
        
        [Required]
        public int  UserLabId { get; set; }
        [ForeignKey(nameof(UserLabId))]
        public UserLab UserLab { get; set; }

        public int HypervisorVmTemplateId { get; set; }
        [ForeignKey(nameof(HypervisorVmTemplateId))]
        public HypervisorVmTemplate HypervisorVmTemplate { get; set; }
        
        [InverseProperty(nameof(VmInterfaceInstance.UserLabVm))]
        public List<VmInterfaceInstance> InterfaceInstances { get; set; } = new List<VmInterfaceInstance>();


        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<UserLabVm>();
            builder.Entity<UserLabVm>().HasIndex(u => new {u.UserLabId, u.LabVmId}).IsUnique();
            builder.Entity<UserLabVm>().HasIndex(u => new {u.UserLabId});
            builder.Entity<UserLabVm>().HasIndex(u => new {u.LabVmId});
        }

        
    }
}