using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : BaseController
    {
        public MaintenanceController(BaseControllerDependencies deps) : base(deps) { }

        [HttpGet]
        public IActionResult GetMaintenances()
        {
            return Ok(DatabaseContext.Maintenances);
        }
    }
}