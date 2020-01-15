using System.Threading.Tasks;
using CSLabsBackend.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualMachineController : BaseController
    {
        public VirtualMachineController(BaseControllerDependencies deps) : base(deps) { }
        
        // GET
        [HttpGet("get-ticket/{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var vm = await GetVm(id);
            return Ok(await ProxmoxManager.GetProxmoxApi(vm.UserLab).GetTicket(vm.Id));
        }

        //Shutdown
        [HttpPost("shutdown/{id}")]
        public async Task<IActionResult> Shutdown(int id)
        {
            var vm = await GetVm(id);
            if (vm.UserId != GetUser().Id) return Forbid();
            await ProxmoxManager.GetProxmoxApi(vm.UserLab).ShutdownVm(vm.ProxmoxVmId);
            return Ok();
        }
        
        //Shutdown
        [HttpPost("stop/{id}")]
        public async Task<IActionResult> Stop(int id)
        {
            var vm = await GetVm(id);
            if (vm.UserId != GetUser().Id) return Forbid();
            await ProxmoxManager.GetProxmoxApi(vm.UserLab).StopVM(vm.ProxmoxVmId);
            return Ok();
        }

        //StartUp
        [HttpPost("start/{id}")]
        public async Task<IActionResult> StartUp(int id)
        {
            var vm = await GetVm(id);
            if (vm.UserId != GetUser().Id) return Forbid();
            await ProxmoxManager.GetProxmoxApi(vm.UserLab).StartVM(vm.ProxmoxVmId);
            return Ok();
        }

        private async Task<UserLabVm> GetVm(int id)
        {
            return await DatabaseContext.UserLabVms
                .Include(v => v.UserLab)
                .FirstAsync(v => v.Id == id);
        }

        
    }
}