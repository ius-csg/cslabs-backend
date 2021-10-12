using System;
using System.ComponentModel.DataAnnotations;

namespace CSLabs.Api.Models
{
    public interface ITrackable
    {
        DateTime Created_At { get; set; }
        DateTime Updated_At { get; set; }
    }
    public abstract class Trackable
    {
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }
}