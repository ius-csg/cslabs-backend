using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabsBackend.Models;
using CSLabsBackend.Models.UserModels;
using CSLabsBackend.Proxmox.Responses;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Proxmox
{
    public class ProxmoxManager
    {
        private DefaultContext _context;
        public ProxmoxManager(DefaultContext context)
        {
            _context = context;
        }
        public async Task<ProxmoxApi> GetLeastLoadedHyperVisor(long requiredMemoryGB)
        {
            long requiredMemory = requiredMemoryGB * 1024 * 1024 * 1024;
            
            var hypervisorNodes = _context.HypervisorNodes
                .Include(n => n.Hypervisor)
                .ToList();
            var list = new List<KeyValuePair<NodeStatus,ProxmoxApi>>();
            foreach (var hypervisorNode in hypervisorNodes)
            {
                var api = new ProxmoxApi(hypervisorNode);
                var nodeStatus = await api.GetNodeStatus();
                list.Add(new KeyValuePair<NodeStatus, ProxmoxApi>(nodeStatus, api));
            }

            list = list.Where(p => p.Key.MemoryUsage.Free > requiredMemory).ToList();
            list.Sort((s1,s2) => s1.Key.CpuUsage - s2.Key.CpuUsage);
            if(list.Count == 0)
                throw new NoHypervisorAvailableException();

            return list.First().Value;
        }
        
        public ProxmoxApi GetProxmoxApi(UserLab userLab)
        {
            return new ProxmoxApi(userLab.HypervisorNode);
        }
    }
}