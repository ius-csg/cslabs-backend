using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Models.HypervisorModels;

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