using System.Collections.Generic;
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

        public Lab Lab { get; set; }

        [InverseProperty(nameof(VmTemplate.LabVm))]
        public List<VmTemplate> VmTemplates { get; set; } = new List<VmTemplate>();

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<LabVm>();
            builder.Unique<LabVm>(u => u.Name);
        }
    }
}