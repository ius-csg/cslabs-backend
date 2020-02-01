using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using CSLabs.Api.Models;
using CSLabs.Api.Models.Enums;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Proxmox;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserModuleController : BaseController
    {
        public UserModuleController(BaseControllerDependencies deps) : base(deps)
        {
        }

        [HttpPost("{specialCode}")]
        public async Task<IActionResult> Post(string specialCode)
        {
            var module = await DatabaseContext.Modules
                .IncludeRelations()
                .FirstAsync(m => m.SpecialCode == specialCode);
            
            if (module == null)
                return BadRequest(new ErrorResponse {Message = "Module not found"});

            var count = await DatabaseContext.UserModules
                .Where(m => m.ModuleId == module.Id)
                .WhereIncludesUser(GetUser())
                .CountAsync();
            
            if (count > 0)
                return Forbid("Cannot Create Multiple Instances");
            
            UserModule userModule = null;
            
            if (module.Type == EModuleType.MultiUser)
                userModule = await DatabaseContext.UserModules
                    .Include(um => um.UserUserModules)
                    .FirstOrDefaultAsync(um => um.ModuleId == module.Id);
            
            if (userModule == null)
            {
                userModule = new UserModule
                {
                    Module = module,
                    UserUserModules = new List<UserUserModule> {new UserUserModule {User = GetUser()}},
                    UserLabs =  module.Labs.Select(lab => new UserLab
                    {
                        Lab = lab,
                        Status = EUserLabStatus.NotStarted
                    }).ToList()
                };
                DatabaseContext.UserModules.Add(userModule);
            }
            else
            {
                userModule.UserUserModules.Add(new UserUserModule {User = GetUser()});
            }
           
            await DatabaseContext.SaveChangesAsync();
            return Ok(userModule);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DatabaseContext.UserModules
                .WhereIncludesUser(GetUser())
                .Include(u => u.Module)
                .ToList());
        }

        //Get api
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var module = await DatabaseContext.UserModules
                .Include(um => um.Module)
                .Include(um => um.UserLabs)
                .ThenInclude(ul => ul.UserLabVms)
                .ThenInclude(vm => vm.LabVm)
                .Include(um => um.UserLabs)
                .ThenInclude(ul => ul.Lab)
                .WhereIncludesUser(GetUser())
                .FirstOrDefaultAsync(um => um.Id == id);
            if (module == null)
                return NotFound();
            return Ok(module);
        }
    }
}