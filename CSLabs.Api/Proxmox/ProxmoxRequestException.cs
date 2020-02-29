using Corsinvest.ProxmoxVE.Api;

namespace CSLabs.Api.Proxmox
{
    public class ProxmoxRequestException : ProxmoxException
    {
        public Result Response { get; set; }
        
        public ProxmoxRequestException(string message, Result result) : base(message)
        {
            Response = result;
        }
    }
}