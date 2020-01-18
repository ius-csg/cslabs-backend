using System.Threading.Tasks;
using System.Web;
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
            var url = vm.UserLab.HypervisorNode.Hypervisor.NoVncUrl
                .Replace("{node}", vm.UserLab.HypervisorNode.Name)
                .Replace("{vm}", vm.ProxmoxVmId.ToString());
          
            var ticket = await ProxmoxManager.GetProxmoxApi(vm.UserLab).GetTicket(vm.ProxmoxVmId);
            url += "?port=" + ticket.Port + "&vncticket=" + HttpUtility.UrlEncode(ticket.Ticket);
            return Ok(new
            {
                Ticket = ticket.Ticket,
                Port = ticket.Port,
                Url = url
            });
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
        
        //Stop
        [HttpPost("stop/{id}")]
        public async Task<IActionResult> Reset(int id)
        {
            var vm = await GetVm(id);
            if (vm.UserId != GetUser().Id) return Forbid();
            await ProxmoxManager.GetProxmoxApi(vm.UserLab).ResetVM(vm.ProxmoxVmId);
            return Ok();
        }
        
        //Stop
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
                .ThenInclude(l => l.HypervisorNode)
                .ThenInclude(n => n.Hypervisor)
                .FirstAsync(v => v.Id == id);
        }

        
    }
}