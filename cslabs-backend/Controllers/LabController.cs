using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : BaseController
    {
        public LabController(BaseControllerDependencies dependencies) : base(dependencies) { }
        
        public async Task<IActionResult> Get(int id)
        {
            var lab =  DatabaseContext.Labs
                .Include(l => l.LabVms)
                .First(m => m.UserId == GetUser().Id && m.Id == id);
            if (lab == null)
                return NotFound();
            var dic = new Dictionary<int, string>();
            foreach (var vm in lab.LabVms)
            {
                var status = await proxmoxApi.GetVmStatus(vm.TemplateProxmoxVmId);
                dic.Add(vm.Id, status.Status);
            }

            return Ok(dic);
        }
    }
}