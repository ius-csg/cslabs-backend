using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Models.ModuleModels;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CSLabsBackend.Models.UserModels
{
    public class UserLabVm : Trackable
    {
        public int Id { get; set; }
        [Required]
        public int  UserId { get; set; }
        [Required]
        public int  UserLabId { get; set; }
        [Required]
        public int LabVmId { get; set; }
        [Required]
        public int ProxmoxVmId { get; set; }
        
        public UserLab UserLab { get; set; }
        
        public User User { get; set; }
        
        public LabVm LabVm { get; set; }
        
        public int VmTemplateId { get; set; }
        [ForeignKey(nameof(VmTemplateId))]
        public VmTemplate VmTemplate { get; set; }


        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<UserLabVm>();
            builder.Entity<UserLabVm>().HasIndex(u => new {u.UserId, u.LabVmId}).IsUnique();
        }
    }
}