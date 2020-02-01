namespace CSLabs.Api.Proxmox.Responses
{
    public class NodeStatus
    {
        // 0- 100
        public double CpuUsage { get; set; }
        public MemoryUsage MemoryUsage { get; set; }
    }
}