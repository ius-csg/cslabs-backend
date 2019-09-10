using System;
using System.ComponentModel.DataAnnotations;

namespace CSLabsBackend.Models
{
    public interface ITrackable
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}