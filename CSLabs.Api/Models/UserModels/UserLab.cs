using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSLabs.Api.Models.UserModels
{
    public class UserLab : Trackable
    {
        public int Id { get; set; }
        [Required]
        public int  UserId { get; set; }
        [Required]
        public int  UserModuleId { get; set; }
        [Required]
        public int LabId { get; set; }

        public UserModule UserModule { get; set; }
        [InverseProperty(nameof(UserLabVm.UserLab))]
        public List<UserLabVm> UserLabVms { get; set; }
        
        public User User { get; set; }
        public Lab Lab { get; set; }
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public EUserLabStatus Status { get; set; }
        public DateTime? LastUsed { get; set; }
        
        [NotMapped]
        public bool HasTopology { get; set; }
        [NotMapped]
        public bool HasReadme { get; set; }

        public void FillAttachmentProperties()
        {
            HasTopology = System.IO.File.Exists("Assets/Images/" + LabId + ".jpg");
            HasReadme = System.IO.File.Exists("Assets/Pdf/" + LabId + ".pdf");
        }
        
        public int? HypervisorNodeId  { get; set; }
        [ForeignKey(nameof(HypervisorNodeId))]
        [JsonIgnore]
        public HypervisorNode HypervisorNode { get; set; }
        
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.TimeStamps<UserLab>();
            modelBuilder.Entity<UserLab>().HasIndex(u => new {u.UserId, u.LabId}).IsUnique();
            modelBuilder.Entity<UserLab>()
                .HasOne(u => u.HypervisorNode)
                .WithMany(n => n.UserLabs)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserLab>().Property(p => p.Status).HasConversion<string>();
        }
    }
}