namespace CSLabs.Api.Models
{
    public class SystemStatus
    {
        public int HypervisorId { get; set; }
        public bool Quorum { get; set; }
        public int HypervisorNodesUp { get; set; }
        public int TotalNodes { get; set; }
    }
}
