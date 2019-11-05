using System.Threading.Tasks;
using AutoMapper;
using CSLabsBackend.Models;
using CSLabsBackend.Proxmox;
using Microsoft.AspNetCore.Mvc;

namespace CSLabsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualMachinesController : BaseController
    {
        private ProxmoxApi api;
        
        public VirtualMachinesController(DefaultContext defaultContext, IMapper mapper) : base(defaultContext, mapper)
        {
            api = new ProxmoxApi("http://", "***REMOVED***", "***REMOVED***");
        }
        
        // GET
        [HttpGet("get-ticket/{id}")]
        public async Task<IActionResult> GetTicket(int Id)
        {
            return Ok(await api.GetTicket(Id));
        }

        //Shutdown
        [HttpPost("shutdown/{id}")]
        public async Task<IActionResult> Shutdown(int Id)
        {
            var userLabVm = DatabaseContext.UserLabVms.Find(Id);
            if (userLabVm.UserId != GetUser().Id)
                return Forbid();
            await api.ShutdownVm( userLabVm.ProxmoxVmId);
            return Ok();
        }

        //StartUp
        [HttpPost("start/{id}")]
        public async Task<IActionResult> StartUp(int Id)
        {
            var userLabVm = DatabaseContext.UserLabVms.Find(Id);
            if (userLabVm.UserId != GetUser().Id)
                return Forbid();
            await api.StartVM(userLabVm.ProxmoxVmId);
            return Ok();
        }

        
    }
}