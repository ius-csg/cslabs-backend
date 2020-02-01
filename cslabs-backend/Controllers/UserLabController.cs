using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabsBackend.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserLabController : BaseController
    {
        public UserLabController(BaseControllerDependencies dependencies) : base(dependencies) { }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus(int id)
        {
            var userLab =  DatabaseContext.UserLabs
                .Include(l => l.UserLabVms)
                .First(m => m.UserId == GetUser().Id && m.Id == id);
            if (userLab == null)
                return NotFound();
            var dic = new Dictionary<int, string>();
            foreach (var vm in userLab.UserLabVms)
            {
                var status = await proxmoxApi.GetVmStatus(vm.ProxmoxVmId);
                dic.Add(vm.Id, status.Status);
            }

            return Ok(dic);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var lab = await DatabaseContext.UserLabs
                .Include(u => u.Lab)
                .Include(u => u.UserLabVms)
                .ThenInclude(v => v.LabVm)
                .FirstAsync(u => u.UserId == GetUser().Id && u.Id == id);
            
            if (lab.Status == "In Progress")
            {
                var startDate = lab.CreatedAt;
                lab.LabEndTime = startDate.AddDays(30);
                TimeSpan timeDifference = lab.LabEndTime .Subtract(DateTime.Now);

                if (timeDifference == TimeSpan.Zero)
                {
                    lab.Status = "Completed";
                }
            }

            lab.HasTopology = System.IO.File.Exists("Assets/images/" + id + ".jpg");
            lab.HasReadme = System.IO.File.Exists("Assets/Pdf/" + id + ".pdf");
            return Ok(lab);
        }
        
        [AllowAnonymous]
        [HttpGet("{id}/topology")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = System.IO.File.OpenRead("Assets/images/" + id + ".jpg");
            return File(image, "image/jpeg");
        }
        
        [AllowAnonymous]
        [HttpGet("{id}/readme")]
        public async Task<IActionResult> GetDocument(int id)
        {
            var image = System.IO.File.OpenRead("Assets/Pdf/" + id + ".pdf");
            return File(image, "application/pdf");
        }

        /*[HttpGet("{id}")]
        public async Task<IActionResult> LabStatus(int id)
        {
            var lab = await DatabaseContext.UserLabs
                .Include(u => u.Lab)
                .FirstAsync(u => u.UserId == GetUser().Id && u.Id == id);
            if (lab.Status == "ON")
            {
                var startDate = lab.CreatedAt;
                var labDuration = startDate.AddDays(30);
                TimeSpan timeDifference = labDuration.Subtract(DateTime.Now);

                if (timeDifference == TimeSpan.Zero)
                {
                    lab.Status = "Completed";
                }
            }
            return Ok(lab);
        }*/
    }
}