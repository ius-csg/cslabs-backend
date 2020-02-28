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
    public class UserLabVm : Trackable
    {
        public int Id { get; set; }
        
        public int? LabVmId { get; set; }
        [Required]
        public int ProxmoxVmId { get; set; }
        
        [Required]
        public int  UserLabId { get; set; }
        [ForeignKey(nameof(UserLabId))]
        public UserLab UserLab { get; set; }

        public LabVm LabVm { get; set; }
        
        public bool IsRootRouter { get; set; }
        
        public int? VmTemplateId { get; set; }
        [ForeignKey(nameof(VmTemplateId))]
        public VmTemplate VmTemplate { get; set; }
        
        [InverseProperty(nameof(HypervisorNetworkInterface.UserLabVm))]
        public List<HypervisorNetworkInterface> Interfaces { get; set; } = new List<HypervisorNetworkInterface>();


        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<UserLabVm>();
            builder.Entity<UserLabVm>().HasIndex(u => new {u.UserLabId, u.LabVmId}).IsUnique();
            builder.Entity<UserLabVm>().HasIndex(u => new {u.UserLabId});
            builder.Entity<UserLabVm>().HasIndex(u => new {u.LabVmId});
        }
    }
}