using System;

namespace CSLabsBackend.Proxmox
{
    public class ProxmoxException : Exception
    {
        public ProxmoxException(string message) : base(message)
        {
            
        }
    }
}