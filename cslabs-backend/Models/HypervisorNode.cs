using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models
{
    public class HypervisorNode
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public int HypervisorId  { get; set; }
        [ForeignKey(nameof(HypervisorId))]
        public Hypervisor Hypervisor { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Unique<HypervisorNode>(n => new {n.Name, n.HypervisorId});
        }
    }
}