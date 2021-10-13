using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Models;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : BaseController
    {
        public ModuleController(BaseControllerDependencies deps) : base(deps)
        {
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DatabaseContext.Modules.Where(m => m.Published).ToList());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var module = await this.DatabaseContext.Modules
                .Where(m => m.Published)
                .FirstAsync(m => m.Id == id);
            if (!User.Identity.IsAuthenticated) return Ok(module);
            await module.SetUserModuleIdIfExists(DatabaseContext, GetUser());
            return Ok(module);
        }
        
        [HttpGet("code/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string code)
        {
            var module = await this.DatabaseContext.Modules.FirstAsync(m => m.SpecialCode == code);
            await module.SetUserModuleIdIfExists(DatabaseContext, GetUser());
            return Ok(module);
        }
        
        
        [HttpGet("module-editor/{id}")]
        public async Task<IActionResult> GetForModuleEditor(int id)
        {
            var module = await this.DatabaseContext
                .Modules
                .Include(m => m.Labs)
                .FirstAsync(m => m.Id == id);
            var labIds = module.Labs.Select(l => l.Id);
            // efficiently detect if labs have user labs
            var totals = await DatabaseContext.UserLabs.Where(ul => labIds.Contains(ul.LabId)).GroupBy(ul => ul.LabId)
                .Select(ul => new {Total = ul.Count(), LabId = ul.Key})
                .ToDictionaryAsync(ul => ul.LabId);
            foreach (var lab in module.Labs)
            {
                if (totals.ContainsKey(lab.Id)) {
                    lab.HasUserLabs = totals[lab.Id].Total > 0;
                }
            }
            if (!GetUser().CanEditModules()) {
                return Forbid("You are not allowed to edit modules");
            }
            // prevent someone from editing another user's module unless they are admin.
            if (module.OwnerId != GetUser().Id && !GetUser().IsAdmin()) {
                return Forbid("You are not allowed to edit this module");
            }
            return Ok(module);
        }

        // GET api/module/modules-editor
        [HttpGet("modules-editor")]
        public async Task<IActionResult> GetUsersModules()
        {
            if (!GetUser().CanEditModules()) {
                return Forbid("You are not allowed to edit modules");
            }
            var query = this.DatabaseContext.Modules.AsQueryable();
            if (!GetUser().IsAdmin())
                query = query.Where(m => m.OwnerId == GetUser().Id);
            return Ok(await query.ToListAsync());
        }
        
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] Module module)
        {
            if (!GetUser().CanEditModules()) {
                return Forbid("You are not allowed to edit modules");
            }
            // prevent someone from editing another user's module unless they are admin.
            if (module.Id != 0 && module.OwnerId != GetUser().Id && !GetUser().IsAdmin()) {
                return Forbid("You are not allowed to edit this module");
            }
            // do not affect labs when saved, labs are saved in a separate request
            module.Labs = null;
            DatabaseContext.Entry(module).State = EntityState.Modified;
            module.Owner = GetUser();
            if (module.Id != 0)
                DatabaseContext.Update(module);
            else
                DatabaseContext.Add(module);
            await DatabaseContext.SaveChangesAsync();
            await DatabaseContext.Entry(module).Collection(m => m.Labs).LoadAsync();
            return Ok(module);
        }
    }
}
