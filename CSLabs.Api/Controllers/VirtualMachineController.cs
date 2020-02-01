using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CSLabs.Api.Models;
using CSLabs.Api.Models.UserModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ResultOperators.Internal;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualMachineController : BaseController
    {
        public VirtualMachineController(BaseControllerDependencies deps) : base(deps) { }
        
        // GET
        [HttpGet("{id}/get-ticket")]
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
        [HttpPost("{id}/shutdown")]
        public async Task<IActionResult> Shutdown(int id)
        {
            var vm = await GetVm(id);
            var api = ProxmoxManager.GetProxmoxApi(vm.UserLab);
            await api.ShutdownVm(vm.ProxmoxVmId);
            return Ok();
        }

        [HttpPost("{id}/scrub")]
        public async Task<IActionResult> Scrub(int id)
        {
            var vm = await DatabaseContext.UserLabVms
                .Include(l => l.VmTemplate)
                .ThenInclude(l => l.HypervisorNode)
                .ThenInclude(l => l.Hypervisor)
                .WhereIncludesUser(GetUser())
                .FirstAsync(v => v.Id == id);
            var api = ProxmoxManager.GetProxmoxApi(vm.VmTemplate.HypervisorNode);
            await api.DestroyVm(vm.ProxmoxVmId);
            await api.CloneTemplate(api.HypervisorNode, vm.VmTemplate.TemplateVmId, vm.ProxmoxVmId);
            var status = await api.GetVmStatus(vm.ProxmoxVmId);
            while (status.Lock == "clone")
            {
                status = await api.GetVmStatus(vm.ProxmoxVmId);
            }
            await api.StartVM(vm.ProxmoxVmId);
            return Ok();
        }
        
        //Reset
        [HttpPost("{id}/reset")]
        public async Task<IActionResult> Reset(int id)
        {
            var vm = await GetVm(id);
            await ProxmoxManager.GetProxmoxApi(vm.UserLab).ResetVM(vm.ProxmoxVmId);
            return Ok();
        }
        
        //Stop
        [HttpPost("{id}/stop")]
        public async Task<IActionResult> Stop(int id)
        {
            var vm = await GetVm(id);
            await ProxmoxManager.GetProxmoxApi(vm.UserLab).StopVM(vm.ProxmoxVmId);
            return Ok();
        }

        //StartUp
        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartUp(int id)
        {
            var vm = await GetVm(id);
            await ProxmoxManager.GetProxmoxApi(vm.UserLab).StartVM(vm.ProxmoxVmId);
            return Ok();
        }

        private async Task<UserLabVm> GetVm(int id)
        {
            return await DatabaseContext.UserLabVms
                .Include(v => v.UserLab)
                .ThenInclude(l => l.HypervisorNode)
                .ThenInclude(n => n.Hypervisor)
                .WhereIncludesUser(GetUser())
                .FirstAsync(v => v.Id == id);
        }

        
    }
}