using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserLabController : BaseController
    {
        public UserLabController(BaseControllerDependencies dependencies) : base(dependencies) { }

        [HttpPost("{id}/last-used")]
        public async Task<IActionResult> UpdateLastUsed(int id)
        {
            var userLab = await DatabaseContext.UserLabs
                .FirstAsync(m => m.UserId == GetUser().Id && m.Id == id);
            userLab.LastUsed = DateTime.UtcNow;
            await DatabaseContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("process-last-used")]
        public async Task<IActionResult> ProcessLastUsed()
        {
            var userLabs = await DatabaseContext.UserLabs
                .Include(l => l.HypervisorNode)
                .ThenInclude(n => n.Hypervisor)
                .Include(l => l.UserLabVms)
                .Where(l => l.LastUsed != null && l.LastUsed < DateTime.UtcNow.AddMinutes(-30))
                .ToListAsync();
            foreach (var userLab in userLabs)
            {
                var api = ProxmoxManager.GetProxmoxApi(userLab);
                foreach (var userLabVm in userLab.UserLabVms)
                {
                    await api.ShutdownVm(userLabVm.ProxmoxVmId);
                }
            }
            return Ok();
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> Get(int id)
        {
            var userLab = await DatabaseContext.UserLabs
                .Include(l => l.HypervisorNode)
                .ThenInclude(n => n.Hypervisor)
                .Include(l => l.UserLabVms)
                .FirstAsync(m => m.UserId == GetUser().Id && m.Id == id);
            if (userLab == null)
                return NotFound();
            var dic = new Dictionary<int, string>();
            var api = ProxmoxManager.GetProxmoxApi(userLab);
            foreach (var vm in userLab.UserLabVms)
            {
                var status = await api.GetVmStatus(vm.ProxmoxVmId);
                dic.Add(vm.Id, status.Status);
            }

            return Ok(dic);
        }
    }
}