using System;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Proxmox;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : BaseController
    {
        public HealthCheckController(BaseControllerDependencies dependencies) : base(dependencies)
        {
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var count = await DatabaseContext.Users.CountAsync();
            return Ok("Everything seems to be operational, user count: " + count);
        }
    }
}