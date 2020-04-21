using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Config;
using CSLabs.Api.Models;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Proxmox.Responses;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Proxmox
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
        public async Task<HypervisorNode> GetLeastLoadedHyperVisorNode(Lab lab)
        {
            return await GetLeastLoadedHyperVisorNode(lab.GetFirstAvailableHypervisor(), lab.EstimatedMemoryUsedMb);
        }

        public async Task<HypervisorNode> GetPrimaryHypervisorNode(Hypervisor hypervisor)
        {
            return await _context.HypervisorNodes
                .Where(n => n.HypervisorId == hypervisor.Id && n.Primary)
                .FirstAsync();
        }
        
        public async Task<HypervisorNode> GetLeastLoadedHyperVisorNode(Hypervisor hypervisor, int estimatedMemoryUsedMb)
        {
            long requiredMemoryBytes = estimatedMemoryUsedMb * 1024 * 1204;
            // only support one cluster right now

            var hypervisorNodes = _context.HypervisorNodes
                .Where(n => n.HypervisorId == hypervisor.Id)
                .Include(n => n.Hypervisor)
                .ToList();
            var api = GetProxmoxApi(hypervisorNodes.First());
            var list = new List<KeyValuePair<NodeStatus,HypervisorNode>>();
            foreach (var hypervisorNode in hypervisorNodes)
            {
                var nodeStatus = await api.GetNodeStatus(hypervisorNode);
                list.Add(new KeyValuePair<NodeStatus, HypervisorNode>(nodeStatus, hypervisorNode));
            }

            list = list.Where(p => p.Key.MemoryUsage.Free > requiredMemoryBytes).ToList();
            list.Sort((s1, s2) => (int)((s1.Key.CpuUsage - s2.Key.CpuUsage) * 100));
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
        
        public string GetProxmoxPassword(Hypervisor hypervisor)
        {
            return Cryptography.DecryptString(hypervisor.Password, _encryptionKey);
        }
    }
}