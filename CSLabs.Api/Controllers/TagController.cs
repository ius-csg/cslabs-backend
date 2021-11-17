using System;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models.ModuleModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : BaseController
    {
        public TagController(BaseControllerDependencies deps) : base(deps)
        {
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DatabaseContext.Tags.ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var tag = await this.DatabaseContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null) 
                return BadRequest();
            return Ok(tag);
        }

        [HttpGet("/tag/{tag}")]
        public async Task<IActionResult> Get(String tag)
        {
            var searchTerm = "%" + tag + "%";
            return Ok(
                await DatabaseContext.Tags
                    .Where(t => EF.Functions.Like(t.Name, searchTerm))
                    .ToListAsync());
        }
    }
}