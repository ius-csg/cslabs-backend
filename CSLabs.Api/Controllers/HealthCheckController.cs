using System;
using System.Linq;
using System.Threading.Tasks;
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
            try
            {
                foreach (var hypervisor in hypervisors)
                {
                    var api = ProxmoxManager.GetProxmoxApi(hypervisor.HypervisorNodes.First());
                    var clusterStatus = api.GetClusterStatus();
                    if (!clusterStatus.Result.Quorate)
                    {
                        throw new NoQuorumException();
                        //save value in db that says we are down
                    }

                    foreach (var node in hypervisor.HypervisorNodes)
                    {
                        await api.GetNodeStatus(node);
                    }

                }
            }
            catch (ProxmoxRequestException e)
            {
                //save that a node is down
            }
            catch (NoQuorumException)
            {
                // save that we have no quorum
            }

            return Ok("All Hypervisors are up and responding");
        }

       
    }
}