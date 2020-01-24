using System;

namespace CSLabs.Api.Proxmox
{
    public class ProxmoxException : Exception
    {
        public ProxmoxException(string message) : base(message)
        {
            
        }
    }
}