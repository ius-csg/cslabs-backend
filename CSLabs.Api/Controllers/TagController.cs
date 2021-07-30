using System.Linq;
using System.Threading.Tasks;
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
    }
}