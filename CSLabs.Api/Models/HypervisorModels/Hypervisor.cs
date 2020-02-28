using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models.HypervisorModels
{
    public class Hypervisor
    {
        public int Id { get; set; }
        public string Host { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NoVncUrl { get; set; }
        
        [InverseProperty(nameof(HypervisorNode.Hypervisor))]
        public List<HypervisorNode> HypervisorNodes { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Unique<Hypervisor>(h => h.NoVncUrl);
            modelBuilder.Unique<Hypervisor>(h => h.Host);
        }
    }
}