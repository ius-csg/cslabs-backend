using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LabController : BaseController
    {
        public LabController(BaseControllerDependencies dependencies) : base(dependencies)
        {
        }

        [HttpGet("lab-editor/{id}")]
        public async Task<IActionResult> GetLabForm(int id)
        {
            if (!GetUser().CanEditModules()) {
                return Forbid("You are not allowed to edit labs");
            }

            var lab = await DatabaseContext.Labs
                .Include(l => l.Module)
                .Include(l => l.BridgeTemplates)
                .Include(l => l.LabVms)
                .ThenInclude(l => l.TemplateInterfaces)
                .FirstOrDefaultAsync(l => l.Id == id);
            // prevent someone from editing another user's module unless they are admin.
            if (lab.Module.OwnerId != GetUser().Id && !GetUser().IsAdmin()) {
                return Forbid("You are not allowed to edit this lab");
            }
            return Ok(lab);
        }
        
    }
}