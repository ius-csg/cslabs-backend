using System;

namespace CSLabs.Api.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}