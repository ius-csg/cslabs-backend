using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;
using static CSLabsBackend.Models.Enums.LabTypes;

namespace CSLabsBackend.Models.ModuleModels
{
    public class Lab : Trackable
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(45)")]
        public string LabType { get; set; } = Permanent;
        
        public string ReadMe { get; set; }
        
        public byte[] Image { get; set; }

        [Required]
        public int ModuleId { get; set; }

        // The user who created the Lab
        [Required]
        public int UserId { get; set; }
        public int LabDifficulty { get; set; }
        
        public List<LabVm> LabVms { get; set; }
        
        public int EstimatedCpusUsed { get; set; }
        public int EstimatedMemoryUsedMb { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<Lab>();
            builder.Unique<Lab>(u => u.Name);
        }
    }
}
