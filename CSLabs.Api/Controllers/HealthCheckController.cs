using System;
using System.Linq;
using System.Threading.Tasks;
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
            foreach (var hypervisor in hypervisors)
            {
                var api = ProxmoxManager.GetProxmoxApi(hypervisor.HypervisorNodes.First());
                foreach (var node in hypervisor.HypervisorNodes)
                {
                    await api.GetNodeStatus(node);
                }
               
            }

            return Ok("All Hypervisors are up and responding");
        }

       
    }
}