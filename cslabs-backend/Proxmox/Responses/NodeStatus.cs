namespace CSLabsBackend.Proxmox.Responses
{
    public class NodeStatus
    {
        // 0- 100
        public int CpuUsage { get; set; }
        public MemoryUsage MemoryUsage { get; set; }
    }
}