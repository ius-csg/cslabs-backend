using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Corsinvest.ProxmoxVE.Api;
using Corsinvest.ProxmoxVE.Api.Extension.Info;
using CSLabs.Api.Models;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Proxmox.Responses;
using Newtonsoft.Json;

namespace CSLabs.Api.Proxmox
{
    public class ProxmoxDBApi : ProxmoxApi
    {
        private DefaultContext _context;
        public ProxmoxDBApi(HypervisorNode hypervisorNode, string password, DefaultContext context) : base(hypervisorNode, password)
        {
            _context = context;
        }

        public new async Task StartVM(int vmId, string targetNode = null)
        {
            await base.StartVM(vmId, targetNode);
            // Save in the database that the VM is started
            _context.UserLabVms.Find(vmId).Running = true;
        }
        
        public new async Task StopVM(int vmId)
        {
            await base.StopVM(vmId);
            // Save in the database that the VM is stopped
            _context.UserLabVms.Find(vmId).Running = false;
        }
        
        public new async Task ShutdownVm(int vmId, int timeout = 20)
        {
            await base.ShutdownVm(vmId, timeout);
            // Save in the database that the VM is stopped
            _context.UserLabVms.Find(vmId).Running = false;
        }
    }
}