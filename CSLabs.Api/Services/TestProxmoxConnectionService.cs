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
                        // Zac said change this, no quorum is when 50% or less of the nodes are up
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

        public async Task<IActionResult> TestLabVmConnection()
        {
            var hypervisors = await Context.Hypervisors
                .Include(h => h.HypervisorNodes)
                .ToListAsync();
        
            var labVms = await Context.LabVms
                .Include(v => v.Id)
                .ToListAsync();

            foreach (var hypervisor in hypervisors) 
            {
                var api = ProxmoxManager.GetProxmoxApi(hypervisor.HypervisorNodes.First());
                
                foreach (var labVm in labVms) 
                { 
                    try 
                    { 
                        var vmStatus = await api.GetVmStatus(labVm.Id); 
                        if (vmStatus.IsStopped())  // vm is down
                        { 
                            // attempt to restart the vm
                            try
                            {
                                await api.StartVM(labVm.Id); // 1st attempt
                            }
                            catch (ProxmoxRequestException)
                            {
                                try
                                {
                                    await api.StartVM(labVm.Id); // 2nd attempt
                                }
                                catch (ProxmoxRequestException)
                                {
                                    try
                                    {
                                        //third attempt
                                        await api.ShutdownVm(labVm.Id);
                                        await api.StartVM(labVm.Id);
                                    }
                                    catch (ProxmoxRequestException)
                                    {
                                        // third attempt at restarting has failed. Something really bad has happened
                                        // and the maintainers need to be emailed
                                    }
                                }
                            }
                        }
                    }
                    catch (ProxmoxRequestException) 
                    {
                        
                    }

                }
            }
            
            return new OkObjectResult("All LabVMs are up and responding");
        }
        
    }
}