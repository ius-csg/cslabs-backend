using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.RequestModels;
using CSLabs.Api.Util;
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
            lab.DetectAttachments();
            return Ok(lab);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> UpsertLab([ModelBinder(typeof(JsonWithFilesFormDataModelBinder), Name = "json")] LabRequest labRequest)
        {
            await DatabaseContext.Database.BeginTransactionAsync();
            var lab = Map<Lab>(labRequest);
         
            if (!GetUser().CanEditModules()) {
                return Forbid("You are not allowed to edit labs");
            }
            var module = await DatabaseContext.Modules.Where(m => m.Id == lab.ModuleId).FirstOrDefaultAsync();
            // prevent someone from editing another user's module unless they are admin.
            if (lab.Id != 0 && module.OwnerId != GetUser().Id && !GetUser().IsAdmin()) {
                return Forbid("You are not allowed to edit this module");
            }

            lab.LinkBridgeTemplates();
            
            if (lab.Id == 0)
                await DatabaseContext.Labs.AddAsync(lab);
            else
            {
                await lab.DeleteMissingProperties(DatabaseContext);
                DatabaseContext.Labs.Update(lab);
            }
            await DatabaseContext.SaveChangesAsync();
            if (labRequest.Topology != null)
            {

                var stream = System.IO.File.Open(lab.GetTopologyPath(), FileMode.Create);
                await labRequest.Topology.CopyToAsync(stream);
                stream.Close();
            }
            if (labRequest.Readme != null)
            {
                var stream = System.IO.File.Open(lab.GetReadmePath(), FileMode.Create);
                await labRequest.Readme.CopyToAsync(stream);
                stream.Close();
            }
            DatabaseContext.Database.CommitTransaction();
            lab.DetectAttachments();
            return Ok(lab);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLab(int id)
        {
            if (!GetUser().CanEditModules()) {
                return Forbid("You are not allowed to edit labs");
            }

            var lab = await DatabaseContext.Labs.FirstAsync(l => l.Id == id);
            var module = await DatabaseContext.Modules.Where(m => m.Id == lab.ModuleId).FirstOrDefaultAsync();
            // prevent someone from editing another user's module unless they are admin.
            if (module.OwnerId != GetUser().Id && !GetUser().IsAdmin()) {
                return Forbid("You are not allowed to edit this module");
            }

            if (await DatabaseContext.UserLabs.Where(ul => ul.Lab == lab).AnyAsync()) {
                return Forbid("You cannot delete a lab with active labs");
            }

            DatabaseContext.Labs.Remove(lab);
            await DatabaseContext.SaveChangesAsync();
            return Ok();
        }
        
        [AllowAnonymous]
        [HttpGet("{id}/topology")]
        public async Task<IActionResult> GetTopology(int id)
        {
            var filePath = "Assets/Images/" + id + ".jpg";
            if (!System.IO.File.Exists(filePath)) {
                return NotFound();
            }
            var file = System.IO.File.OpenRead(filePath);
            return File(file, "image/jpeg");
        }
        
        [AllowAnonymous]
        [HttpGet("{id}/readme")]
        public async Task<IActionResult> GetReadme(int id)
        {
            var filePath = "Assets/Pdf/" + id + ".pdf";
            if (!System.IO.File.Exists(filePath)) {
                return NotFound();
            }
            var file = System.IO.File.OpenRead(filePath);
            return File(file, "application/pdf");
        }
    }
}