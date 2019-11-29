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

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus(int id)
        {
            var userLab =  DatabaseContext.UserLabs
                .Include(l => l.UserLabVms)
                .First(m => m.UserId == GetUser().Id && m.Id == id);
            if (userLab == null)
                return NotFound();
            var dic = new Dictionary<int, string>();
            foreach (var vm in userLab.UserLabVms)
            {
                var status = await proxmoxApi.GetVmStatus(vm.ProxmoxVmId);
                dic.Add(vm.Id, status.Status);
            }

            return Ok(dic);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Ge(int id)
        {
            var lab = DatabaseContext.Labs
                .Include(u => u.LabVms)
                .Include(u => u.ModuleId)
                .First(u => u.UserId == GetUser().Id && u.Id == id);
            return Ok(lab);
        }
    }
}