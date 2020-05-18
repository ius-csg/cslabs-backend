using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserLabController : BaseController
    {
        private readonly UserLabInstantiationService _instantiationService;
        public UserLabController(
            BaseControllerDependencies dependencies,
            UserLabInstantiationService instantiationService) : base(dependencies)
        {
            _instantiationService = instantiationService;
        }
        
        [HttpGet("process-last-used")]
        [AllowAnonymous]
        public async Task<IActionResult> ProcessLastUsed()
        {
            var userLabs = await DatabaseContext.UserLabs
                .IncludeHypervisor()
                .Include(l => l.UserLabVms)
                .Where(l => l.LastUsed != null && l.LastUsed < DateTime.UtcNow.AddMinutes(-30))
                .ToListAsync();
            foreach (var userLab in userLabs)
            {
                var api = ProxmoxManager.GetProxmoxApi(userLab);
                userLab.LastUsed = null;
                await DatabaseContext.SaveChangesAsync();
                foreach (var userLabVm in userLab.UserLabVms)
                {
                    await api.StopVM(userLabVm.ProxmoxVmId);
                }
            }
            return Ok();
        }

        [HttpGet("process-end-date-time")]
        [AllowAnonymous]
        public async Task<IActionResult> ProcessEndDateTime()
        {
            // find userLabs that have an EndDateTime paste the current UtcDateTime.
            var userLabs = await DatabaseContext.UserLabs
                .IncludeRelations()
                .IncludeHypervisor()
                .Include(l => l.UserLabVms)
                .Include(l => l.BridgeInstances)
                .Where(l => l.Status == EUserLabStatus.Started)
                .Where(l => l.EndDateTime < DateTime.UtcNow)
                .ToListAsync();
            
            
            // with the found items, destroy all vms and delete the UserLabVm rows.
            foreach (var userLab in userLabs)
            {
                await _instantiationService.Deconstruct(userLab, ProxmoxManager);
            }
            return Ok();
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var userLab = await DatabaseContext.UserLabs
                .IncludeRelations()
                .IncludeLabHypervisor()
                .Include(ul => ul.Lab)
                .FirstAsync(ul => ul.Id == id);
            if (userLab.UserLabVms.Count > 0)
                // update the UI.
                return Ok(userLab);
            await _instantiationService.Instantiate(userLab, ProxmoxManager);
            await DatabaseContext.SaveChangesAsync();
            return Ok(userLab.GetResponse(Mapper));
        }

        [HttpPost("{id}/turn-on")]
        public async Task<IActionResult> TurnOn(int id)
        {
            var userLab = await DatabaseContext.UserLabs
                .IncludeRelations()
                .IncludeLabHypervisor()
                .Include(ul => ul.Lab)
                .FirstAsync(ul => ul.Id == id);
            var api = ProxmoxManager.GetProxmoxApi(userLab);
            foreach (var userLabVm in userLab.UserLabVms)
            {
                if ((await api.GetVmStatus(userLabVm.ProxmoxVmId)).IsStopped()) {
                    await api.StartVM(userLabVm.ProxmoxVmId);
                }
            }

            return Ok();
        }
        
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var userLab = await DatabaseContext.UserLabs
                .IncludeRelations()
                .IncludeLabHypervisor()
                .Include(ul => ul.Lab)
                .FirstAsync(ul => ul.Id == id);
            if (userLab.UserLabVms.Count == 0)
                return BadRequest(new {Message = "Lab not instantiated"});

            await _instantiationService.Deconstruct(userLab, ProxmoxManager);
            await DatabaseContext.SaveChangesAsync();
            return Ok(userLab.GetResponse(Mapper));
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus(int id)
        {
            var userLab = await DatabaseContext.UserLabs
                .IncludeRelations()
                .WhereIncludesUser(GetUser())
                .FirstAsync(ul =>  ul.Id == id);
            if (userLab == null)
                return NotFound();
            
            userLab.LastUsed = DateTime.UtcNow;
            await DatabaseContext.SaveChangesAsync();

            var dic = new Dictionary<int, string>();
            var api = ProxmoxManager.GetProxmoxApi(userLab);
            foreach (var vm in userLab.UserLabVms)
            {
                var status = await api.GetVmStatus(vm.ProxmoxVmId);
                dic.Add(vm.Id, status.Status);
            }

            return Ok(dic);
        }
        
        
        [HttpGet("{id}/initialization-status")]
        public async Task<IActionResult> Status(int id)
        {
            var userLab = DatabaseContext.UserLabs
                .Include(l => l.HypervisorNode)
                .ThenInclude(n => n.Hypervisor)
                .Include(l => l.UserLabVms)
                .WhereIncludesUser(GetUser())
                .FirstOrDefault(m => m.Id == id);
            if (userLab == null)
                return NotFound();
            var api = ProxmoxManager.GetProxmoxApi(userLab);
            foreach (var vm in userLab.UserLabVms)
            {
                var status = await api.GetVmStatus(vm.ProxmoxVmId);
                if (status.Lock == "clone")
                    return Ok("Initializing");
            }

            return Ok("Initialized");
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userLab = await DatabaseContext.UserLabs
                .Include(u => u.Lab)
                .Include(u => u.UserLabVms)
                .ThenInclude(v => v.LabVm)
                .WhereIncludesUser(GetUser())
                .FirstAsync(u => u.Id == id);
            userLab.FillAttachmentProperties();
            return Ok(userLab.GetResponse(Mapper));
        }
    }
}