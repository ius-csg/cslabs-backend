using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models
{
    public class Lab : ITrackable
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }

        [Required]
        public int LabType { get; set; }

        [Required]
        public int ModuleId { get; set; }

        [Required]
        public int CreatorId { get; set; }
        public int LabDifficulty { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string RundeckDestroyUrl { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string RundeckCreateUrl { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string RundeckStartUrl { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string RundeckStopUrl { get; set; }

        [Column(TypeName = "VARCHAR(45)")]
        public string RundeckSnapshotUrl { get; set; }

        [Column(TypeName = "VARCHAR(45)")]
        public string RundeckRestoreSnapshotUrl { get; set; }

        public int EstimatedCpusUsed { get; set; }
        public int EstimatedMemoryUsedMb { get; set; }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Lab>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("UTC_TIMESTAMP()");

            builder.Entity<Lab>().HasIndex(u => u.Name).IsUnique();
        }
    }
}
