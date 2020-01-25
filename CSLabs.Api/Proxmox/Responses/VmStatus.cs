namespace CSLabs.Api.Proxmox.Responses
{
    public class VmStatus
    {
        public string Status { get; set; }
        public string Lock { get; set; }

        public bool IsStopped()
        {
            return Status == "stopped";
        }
    }
}