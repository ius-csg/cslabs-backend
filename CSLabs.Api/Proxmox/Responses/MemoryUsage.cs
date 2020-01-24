namespace CSLabs.Api.Proxmox.Responses
{
    public class MemoryUsage
    {
        // in bytes
        public long Free { get; set; }
        // in bytes
        public long Total { get; set; }
        // in bytes
        public long Used { get; set; }
    }
}