using System;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Proxmox;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : BaseController
    {
        public HealthCheckController(BaseControllerDependencies dependencies) : base(dependencies)
        {
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var count = await DatabaseContext.Users.CountAsync();
            return Ok("Everything seems to be operational, user count: " + count);
        }

        [HttpGet("proxmox")]
        public async Task<IActionResult> TestProxmoxConnection()
        {
            var hypervisors = await DatabaseContext.Hypervisors
                .Include(h => h.HypervisorNodes)
                .ToListAsync();
            var systemStatus = DatabaseContext.SystemStatuses.First(); // Get current system status
            try
            {
                foreach (var hypervisor in hypervisors)
                {
                    var nodesUp = 0;
                    var api = ProxmoxManager.GetProxmoxApi(hypervisor.HypervisorNodes.First());

                    try 
                    {
                        var clusterStatus = api.GetClusterStatus();
                        if (!clusterStatus.Result.Quorate)
                        {
                            throw new NoQuorumException();
                        }
                    }
                    catch (ProxmoxRequestException e)
                    {
                        // GetClusterStatus failed, so no nodes are up 
                        systemStatus.HypervisorNodesUp = 0;
                        await DatabaseContext.SaveChangesAsync();
                    }
                    
                    // check the status of all the nodes and count how many are up
                    foreach (var node in hypervisor.HypervisorNodes)
                    {
                        try
                        {
                            await api.GetNodeStatus(node);
                        }
                        catch (ProxmoxRequestException e)
                        {
                    
                        }

                        nodesUp++;
                    }
                    
                    systemStatus.HypervisorNodesUp = nodesUp;
                    await DatabaseContext.SaveChangesAsync();
                }
            }
            catch (NoQuorumException)
            {
                // save that we have no quorum
                systemStatus.Quorum = false;
                await DatabaseContext.SaveChangesAsync();
            }

            return Ok("All Hypervisors are up and responding");
        }

       
    }
}