using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models.ModuleModels
{
    public class LabVm: Trackable
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }
        [Required]
        public int LabId { get; set; }
        
        [Required]
        public int TemplateProxmoxVmId { get; set; }

        public Lab Lab { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<LabVm>();
            builder.Unique<LabVm>(u => u.Name);
        }
    }
}