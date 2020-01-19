using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabsBackend.Config;
using CSLabsBackend.Models;
using CSLabsBackend.Models.ModuleModels;
using CSLabsBackend.Models.UserModels;
using CSLabsBackend.Proxmox.Responses;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Proxmox
{
    public class ProxmoxManager
    {
        private DefaultContext _context;
        private string _encryptionKey;
        public ProxmoxManager(DefaultContext context, AppSettings appSettings)
        {
            _context = context;
            _encryptionKey = appSettings.ProxmoxEncryptionKey;
        }
        public async Task<ProxmoxApi> GetLeastLoadedHyperVisor(Lab lab)
        {
            long requiredMemoryBytes = lab.EstimatedMemoryUsedMb * 1024 * 1024;
            var firstLabVm = lab.LabVms.First();
            var hypervisor = firstLabVm.VmTemplates.Select(t => t.HypervisorNode.Hypervisor).First();
            var hypervisorNodes = _context.HypervisorNodes
                .Where(n => n.HypervisorId == hypervisor.Id)
                .Include(n => n.Hypervisor)
                .ToList();
            var list = new List<KeyValuePair<NodeStatus,ProxmoxApi>>();
            foreach (var hypervisorNode in hypervisorNodes)
            {
                var api = GetProxmoxApi(hypervisorNode);
                var nodeStatus = await api.GetNodeStatus();
                list.Add(new KeyValuePair<NodeStatus, ProxmoxApi>(nodeStatus, api));
            }

            list = list.Where(p => p.Key.MemoryUsage.Free > requiredMemoryBytes).ToList();
            list.Sort((s1,s2) => (int)(s1.Key.CpuUsage - s2.Key.CpuUsage));
            if(list.Count == 0)
                throw new NoHypervisorAvailableException();

            return list.First().Value;
        }

        public ProxmoxApi GetProxmoxApi(HypervisorNode node)
        {
            var password = Cryptography.DecryptString(node.Hypervisor.Password, _encryptionKey);
            return new ProxmoxApi(node, password);
        }
        
        public ProxmoxApi GetProxmoxApi(UserLab userLab)
        {
            return GetProxmoxApi(userLab.HypervisorNode);
        }
    }
}