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
        public async Task<IActionResult> Get(int id)
        {
            var userLab =  DatabaseContext.UserLabs
                .Include(l => l.UserLabVms)
                .First(m => m.UserId == GetUser().Id && m.Id == id);
            if (userLab == null)
                return NotFound();
            var dic = new Dictionary<int, string>();
            foreach (var vm in userLab.UserLabVms)
            {
                var status = await proxmoxApi.GetVmStatus("a1", vm.ProxmoxVmId);
                dic.Add(vm.Id, status.Status);
            }

            return Ok(dic);
        }
    }
}