using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabsBackend.Models.ModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : BaseController
    {
        public LabController(BaseControllerDependencies dependencies) : base(dependencies) { }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DatabaseContext.Labs
                .Include(u => u.LabVms)
                .ToList());
        }
        
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var lab = DatabaseContext.Labs
                .Include(u => u.LabVms)
                .Include(u => u.ModuleId)
                .First(u => u.UserId == GetUser().Id && u.Id == id);
            return Ok(lab);
        }
    }
}