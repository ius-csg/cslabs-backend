using System.Threading.Tasks;
using AutoMapper;
using CSLabsBackend.Models;
using CSLabsBackend.Proxmox;
using Microsoft.AspNetCore.Mvc;

namespace CSLabsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualMachineController : BaseController
    {
        public VirtualMachineController(BaseControllerDependencies deps) : base(deps) { }
        
        // GET
        [HttpGet("get-ticket/{id}")]
        public async Task<IActionResult> GetTicket(int Id)
        {
            return Ok(await proxmoxApi.GetTicket(Id));
        }

        //Shutdown
        [HttpPost("shutdown/{id}")]
        public async Task<IActionResult> Shutdown(int Id)
        {
            var userLabVm = DatabaseContext.UserLabVms.Find(Id);
            if (userLabVm.UserId != GetUser().Id)
                return Forbid();
            await proxmoxApi.ShutdownVm( userLabVm.ProxmoxVmId);
            return Ok();
        }

        //StartUp
        [HttpPost("start/{id}")]
        public async Task<IActionResult> StartUp(int Id)
        {
            var userLabVm = DatabaseContext.UserLabVms.Find(Id);
            if (userLabVm.UserId != GetUser().Id)
                return Forbid();
            await proxmoxApi.StartVM(userLabVm.ProxmoxVmId);
            return Ok();
        }

        
    }
}