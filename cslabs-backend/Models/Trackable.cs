using System;
using System.ComponentModel.DataAnnotations;

namespace CSLabsBackend.Models
{
    public abstract class Trackable
    {
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}