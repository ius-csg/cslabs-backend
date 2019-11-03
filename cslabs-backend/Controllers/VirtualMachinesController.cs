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
        public VirtualMachinesController(DefaultContext defaultContext, IMapper mapper) : base(defaultContext, mapper)
        {
        }
        
        // GET
        [HttpGet("get-ticket/{id}")]
        public async Task<IActionResult> GetTicket(int Id)
        {
            var api = new ProxmoxApi("http://", "192.168.1.160:4440", "AbLawQG9c2Amk4OaeP7h6NxaZZnTdwMP");
            return Ok(await api.GetTicket(Id));
        }

        
    }
}