using System;
using System.ComponentModel.DataAnnotations;

namespace CSLabsBackend.Models
{
    public abstract class ITrackable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}