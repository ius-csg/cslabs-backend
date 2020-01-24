using System.ComponentModel.DataAnnotations;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models
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

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Unique<Hypervisor>(h => h.NoVncUrl);
            modelBuilder.Unique<Hypervisor>(h => h.Host);
        }
    }
}