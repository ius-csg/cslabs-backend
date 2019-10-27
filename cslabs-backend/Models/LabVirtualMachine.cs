using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models
{
    public class LabVirtualMachine: ITrackable
    {
        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }
        [Required]
        public int LabId { get; set; }

        public Lab Lab { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<LabVirtualMachine>();
        }
    }
}