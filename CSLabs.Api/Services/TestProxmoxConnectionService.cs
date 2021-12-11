using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Proxmox;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Services
{
    public class TestProxmoxConnectionService
    {
        private DefaultContext Context { get; }

        private ProxmoxManager ProxmoxManager;
        
        public TestProxmoxConnectionService(DefaultContext context)
        {
            Context = context;
        }
        
        public async Task<IActionResult> TestProxmoxConnection()
        {
            var hypervisors = await Context.Hypervisors
                .Include(h => h.HypervisorNodes)
                .ToListAsync();
            var systemStatus = Context.SystemStatuses.First(); // Get current system status
            try
            {
                foreach (var hypervisor in hypervisors)
                {
                    var nodesUp = 0;
                    var api = ProxmoxManager.GetProxmoxApi(hypervisor.HypervisorNodes.First());

                    try 
                    {
                        var clusterStatus = await api.GetClusterStatus();
                        if (!clusterStatus.Quorate)
                        {
                            throw new NoQuorumException();
                        }
                    }
                    catch (ProxmoxRequestException)
                    {
                        // GetClusterStatus failed, so no nodes are up 
                        systemStatus.HypervisorNodesUp = 0;
                        await Context.SaveChangesAsync();
                    }
                    
                    // check the status of all the nodes and count how many are up
                    foreach (var node in hypervisor.HypervisorNodes)
                    {
                        try
                        {
                            await api.GetNodeStatus(node);
                            nodesUp++;
                        }
                        catch (ProxmoxRequestException)
                        {
                            // do not increment nodes up if the request fails.
                        }
                    }
                    
                    systemStatus.HypervisorNodesUp = nodesUp;
                    await Context.SaveChangesAsync();
                }
            }
            catch (NoQuorumException)
            {
                // save that we have no quorum
                systemStatus.Quorum = false;
                await Context.SaveChangesAsync();
            }

            return new OkObjectResult("All Hypervisors are up and responding");
        }
    }
}