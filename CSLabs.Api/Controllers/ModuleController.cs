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
using Microsoft.IdentityModel.Tokens;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : BaseController
    {
        public ModuleController(BaseControllerDependencies deps) : base(deps)
        {
        }
        
        // GET api/v1/Modules/?tagNames
        [HttpGet]
        public async Task<IActionResult> Get(string tagNames = "")
        {
            if (string.IsNullOrEmpty(tagNames))
                return Ok(await DatabaseContext.Modules.Where(m => m.Published).IncludeTags().ToListAsync());
            var tagNamesList = tagNames.Split(",").Select(s => s.Trim()).ToList();
            return Ok(
                await DatabaseContext.Modules
                    .Where(m => m.Published)
                    .Where(m => m.ModuleTags.Any(mt => tagNamesList.Contains(mt.Tag.Name)))
                    .IncludeTags()
                    .ToListAsync()
            );
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var module = await this.DatabaseContext.Modules
                .Where(m => m.Published)
                .IncludeTags()
                .FirstAsync(m => m.Id == id);
            if (!User.Identity.IsAuthenticated) return Ok(module);
            await module.SetUserModuleIdIfExists(DatabaseContext, GetUser());
            return Ok(module);
        }
        
        [HttpGet("code/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCode(string code)
        {
            var module = await this.DatabaseContext.Modules
                .IncludeTags()
                .FirstAsync(m => m.SpecialCode == code);
            await module.SetUserModuleIdIfExists(DatabaseContext, GetUser());
            return Ok(module);
        }
        
        [HttpGet("{id}/tags")]
        public async Task<IActionResult> GetTags(int id) =>
            Ok(await DatabaseContext.Tags.Where(t => t.ModuleTags.Any(mt => mt.ModuleId == id)).ToListAsync());
        
        [HttpGet("module-editor/{id}")]
        public async Task<IActionResult> GetForModuleEditor(int id)
        {
            var module = await this.DatabaseContext
                .Modules
                .Include(m => m.Labs)
                .IncludeTags()
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
        public async Task<IActionResult> GetEditorsModules()
        {
            if (!GetUser().CanEditModules()) {
                return Forbid("You are not allowed to edit modules");
            }
            var query = this.DatabaseContext.Modules.AsQueryable();
            if (!GetUser().IsAdmin())
                query = query.Where(m => m.OwnerId == GetUser().Id).IncludeTags();
            return Ok(await query.ToListAsync());
        }
        
        // GET api/module/modules-editor/search/{searchTerm}
        [HttpGet("modules-editor/search/{searchTerm}")]
        public async Task<IActionResult> SearchEditorsModules(string searchTerm)
        {
            if (!GetUser().CanEditModules())
                return Forbid("You are not allowed to edit modules");
            
            if (string.IsNullOrEmpty(searchTerm))
                return Ok(await DatabaseContext.Modules.ToListAsync());

            var searchedModules = DatabaseContext.Modules.AsQueryable();
            if (!GetUser().IsAdmin())
                searchedModules = searchedModules.Where(m => m.OwnerId == GetUser().Id);
            
            searchedModules = searchedModules
                .Where(m => EF.Functions.Match(m.Name, searchTerm, MySqlMatchSearchMode.NaturalLanguage) ||
                            EF.Functions.Match(m.Description, searchTerm, MySqlMatchSearchMode.NaturalLanguage) ||
                            EF.Functions.Like(m.Name, $"%{searchTerm}%") ||
                            EF.Functions.Like(m.Description, $"%{searchTerm}%") || 
                            m.ModuleTags.Any(mt => searchTerm.Equals(mt.Tag.Name.ToLower())));
            
            return Ok(await searchedModules.ToListAsync());
        }
        
        // GET api/module/modules-editor/search?params={...}
        [HttpGet("modules-editor/search")]
        public async Task<IActionResult> SearchOptionsEditorsModules(string title = null, string description = null, int difficulty = 0, string tags = null)
        {
            if (!GetUser().CanEditModules())
                return Forbid("You are not allowed to edit modules");
            
            var searchedModules = DatabaseContext.Modules.AsQueryable();
            if (GetUser().IsAdmin())
                searchedModules = searchedModules.Where(m => m.OwnerId == GetUser().Id);
            
            if (!string.IsNullOrEmpty(title))
                searchedModules = searchedModules
                    .Where(m => EF.Functions.Match(m.Name, title, MySqlMatchSearchMode.NaturalLanguage) || 
                                EF.Functions.Like(m.Name, $"%{title}%"));

            if (!string.IsNullOrEmpty(description))
                searchedModules = searchedModules
                    .Where(m => EF.Functions.Match(m.Description, description, MySqlMatchSearchMode.NaturalLanguage) || 
                                EF.Functions.Like(m.Description, $"%{description}%"));

            if (difficulty != 0)
                searchedModules = searchedModules
                    .Where(m => m.Difficulty == difficulty);

            if (!string.IsNullOrEmpty(tags))
            {
                var tagNamesList = tags.Split(",").Select(s => s.Trim()).ToList();
                searchedModules = searchedModules
                    .Where(m => m.ModuleTags.Any(mt => tagNamesList.Contains(mt.Tag.Name)))
                    .IncludeTags();
            }
            return Ok(await searchedModules.ToListAsync());
        }

        // GET api/module/search/{searchTerm}
        [HttpGet("search/{searchTerm}")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return Ok(await DatabaseContext.Modules.Where(m => m.Published).ToListAsync());
            
            var searchedModules = DatabaseContext.Modules
                .Where(m => m.Published)
                .Where(m => EF.Functions.Match(m.Name, searchTerm, MySqlMatchSearchMode.NaturalLanguage) || 
                            EF.Functions.Match(m.Description, searchTerm, MySqlMatchSearchMode.NaturalLanguage) ||
                            EF.Functions.Like(m.Name, $"%{searchTerm}%") || 
                            EF.Functions.Like(m.Description, $"%{searchTerm}%") || 
                            m.ModuleTags.Any(mt => searchTerm.Equals(mt.Tag.Name.ToLower())));
            
            return Ok(await searchedModules.ToListAsync());
        }

        // GET api/module/search?params={...}
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchOptions(string title = null, string description = null, int difficulty = 0, string tags = null)
        {
            var searchedModules = DatabaseContext.Modules.Where(m => m.Published);

            if (!string.IsNullOrEmpty(title))
                searchedModules = searchedModules
                    .Where(m => EF.Functions.Match(m.Name, title, MySqlMatchSearchMode.NaturalLanguage) || 
                            EF.Functions.Like(m.Name, $"%{title}%"));

            if (!string.IsNullOrEmpty(description))
                searchedModules = searchedModules
                    .Where(m => EF.Functions.Match(m.Description, description, MySqlMatchSearchMode.NaturalLanguage) || 
                            EF.Functions.Like(m.Description, $"%{description}%"));

            if (difficulty != 0)
                searchedModules = searchedModules
                    .Where(m => m.Difficulty == difficulty);

            if (!string.IsNullOrEmpty(tags))
            {
                var tagNamesList = tags.Split(",").Select(s => s.Trim()).ToList();
                searchedModules = searchedModules
                    .Where(m => m.ModuleTags.Any(mt => tagNamesList.Contains(mt.Tag.Name)))
                    .IncludeTags();
            }
            return Ok(await searchedModules.ToListAsync());
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
            // delete module tags
            module.DeleteModuleTags(DatabaseContext);
            // add tag relationships
            module.AddModuleTags(DatabaseContext);
            await DatabaseContext.SaveChangesAsync();
            await DatabaseContext.Entry(module).Collection(m => m.Labs).LoadAsync();
            return Ok(module);
        }
    }
}
