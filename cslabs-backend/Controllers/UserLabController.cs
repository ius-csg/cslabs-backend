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
        
        [HttpGet("process-last-used")]
        [AllowAnonymous]
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
                userLab.LastUsed = null;
                await DatabaseContext.SaveChangesAsync();
                foreach (var userLabVm in userLab.UserLabVms)
                {
                    await api.ShutdownVm(userLabVm.ProxmoxVmId);
                }
            }
            return Ok();
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus(int id)
        {
            var userLab = await DatabaseContext.UserLabs
                .Include(l => l.HypervisorNode)
                .ThenInclude(n => n.Hypervisor)
                .Include(l => l.UserLabVms)
                .FirstAsync(m => m.UserId == GetUser().Id && m.Id == id);
            if (userLab == null)
                return NotFound();
            
            userLab.LastUsed = DateTime.UtcNow;
            await DatabaseContext.SaveChangesAsync();

            var dic = new Dictionary<int, string>();
            var api = ProxmoxManager.GetProxmoxApi(userLab);
            foreach (var vm in userLab.UserLabVms)
            {
                var status = await api.GetVmStatus(vm.ProxmoxVmId);
                dic.Add(vm.Id, status.Status);
            }

            return Ok(dic);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var lab = DatabaseContext.UserLabs
                .Include(u => u.Lab)
                .Include(u => u.UserLabVms)
                .ThenInclude(v => v.LabVm)
                .First(u => u.UserId == GetUser().Id && u.Id == id);

            lab.HasTopology = System.IO.File.Exists("Assets/Images/" + id + ".jpg");
            lab.HasReadme = System.IO.File.Exists("Assets/Pdf/" + id + ".pdf");
            return Ok(lab);
        }
        
        [AllowAnonymous]
        [HttpGet("{id}/topology")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = System.IO.File.OpenRead("Assets/Images/" + id + ".jpg");
            return File(image, "image/jpeg");
        }
        
        [AllowAnonymous]
        [HttpGet("{id}/readme")]
        public async Task<IActionResult> GetDocument(int id)
        {
            var image = System.IO.File.OpenRead("Assets/Pdf/" + id + ".pdf");
            return File(image, "application/pdf");
        }
    }
}