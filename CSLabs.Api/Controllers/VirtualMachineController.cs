using System.Threading.Tasks;
using System.Web;
using CSLabs.Api.Models;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualMachineController : BaseController
    {
        private UserLabInstantiationService _userLabInstantiation;

        public VirtualMachineController(BaseControllerDependencies deps,
            UserLabInstantiationService userLabInstantiation) : base(deps)
        {
            _userLabInstantiation = userLabInstantiation;
        }
        
        // GET
        [HttpGet("{id}/get-ticket")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var vm = await GetVm(id);
            if (vm == null || vm.IsCoreRouter) {
                return NotFound();
            }
            var url = vm.UserLab.HypervisorNode.Hypervisor.NoVncUrl
                .Replace("{node}", vm.UserLab.HypervisorNode.Name)
                .Replace("{vm}", vm.ProxmoxVmId.ToString());
          
            var ticket = await ProxmoxManager.GetProxmoxApi(vm.UserLab).GetTicket(vm.ProxmoxVmId);
            url += "?port=" + ticket.Port + "&vncticket=" + HttpUtility.UrlEncode(ticket.Ticket);
            return Ok(new
            {
                Ticket = ticket.Ticket,
                Port = ticket.Port,
                Url = url,
                FastBaseUrl = AppSettings.NoVnc.FastBaseUrl,
                ReliableBaseUrl = AppSettings.NoVnc.ReliableBaseUrl,
                HealthCheckUrl = AppSettings.NoVnc.HealthCheckUrl,
                UseHttpsForHealthCheckRequest = AppSettings.NoVnc.UseHttpsForHealthCheckRequest
            });
        }

        //Shutdown
        [HttpPost("{id}/shutdown")]
        public async Task<IActionResult> Shutdown(int id)
        {
            var vm = await GetVm(id);
            if (vm == null) return NotFound();
            var api = ProxmoxManager.GetProxmoxApi(vm.UserLab);
            await api.ShutdownVm(vm.ProxmoxVmId);
            return Ok();
        }

        [HttpPost("{id}/scrub")]
        public async Task<IActionResult> Scrub(int id)
        {
            var vm = await DatabaseContext.UserLabVms
                .Include(l => l.HypervisorVmTemplate)
                .ThenInclude(l => l.HypervisorNode)
                .ThenInclude(l => l.Hypervisor)
                .Include(l => l.UserLab)
                .ThenInclude(l => l.BridgeInstances)
                .WhereIncludesUser(GetUser())
                .FirstAsync(v => v.Id == id);
            if (vm.IsCoreRouter) {
                return NotFound();
            }
            var api = ProxmoxManager.GetProxmoxApi(vm.HypervisorVmTemplate.HypervisorNode);
            await api.DestroyVm(vm.ProxmoxVmId);
            await api.CloneTemplate(api.HypervisorNode, vm.HypervisorVmTemplate.TemplateVmId, vm.ProxmoxVmId);
            await _userLabInstantiation.LinkVmToBridges(vm.UserLab, vm, api, vm.HypervisorVmTemplate.HypervisorNode);
            var status = await api.GetVmStatus(vm.ProxmoxVmId);
            while (status.Lock == "clone") {
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
            if (vm == null || vm.IsCoreRouter) {
                return NotFound();
            }
            await ProxmoxManager.GetProxmoxApi(vm.UserLab).ResetVM(vm.ProxmoxVmId);
            return Ok();
        }
        
        //Stop
        [HttpPost("{id}/stop")]
        public async Task<IActionResult> Stop(int id)
        {
            var vm = await GetVm(id);
            if (vm == null || vm.IsCoreRouter) {
                return NotFound();
            }
            await ProxmoxManager.GetProxmoxApi(vm.UserLab).StopVM(vm.ProxmoxVmId);
            return Ok();
        }

        //StartUp
        [HttpPost("{id}/start")]
        public async Task<IActionResult> StartUp(int id)
        {
            var vm = await GetVm(id);
            if (vm == null || vm.IsCoreRouter) {
                return NotFound();
            }
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
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        
    }
}