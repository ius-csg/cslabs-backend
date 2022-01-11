using System;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Proxmox;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Services
{
    public class ProxmoxApiTokenService
    {
        public ProxmoxManager ProxmoxManager { get;}
        
        public ProxmoxApiTokenService(ProxmoxManager manager)
        {
            ProxmoxManager = manager;
        }

        public async Task ManageApiToken(DefaultContext context)
        {
            var hypervisor = await context.Hypervisors.FirstOrDefaultAsync();
            var api = ProxmoxManager.GetProxmoxApi(hypervisor.HypervisorNodes.First());
            await api.ManageApiToken();
        }
    }
}