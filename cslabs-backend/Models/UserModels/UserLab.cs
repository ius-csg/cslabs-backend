using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Models.ModuleModels;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models.UserModels
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
        
        public string ReadMe { get; set; }
        
        public byte[] Image { get; set; }
        
        public UserModule UserModule { get; set; }
        [InverseProperty(nameof(UserLabVm.UserLab))]
        public List<UserLabVm> UserLabVms { get; set; }
        
        public User User { get; set; }
        public Lab Lab { get; set; }
        [Required]
        public string Status { get; set; }
        
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<UserLab>();
            builder.Entity<UserLab>().HasIndex(u => new {u.UserId, u.LabId}).IsUnique();
        }
    }
}