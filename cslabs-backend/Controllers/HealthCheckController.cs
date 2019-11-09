using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Controllers
{
    public class HealthCheckController : BaseController
    {
        public HealthCheckController(BaseControllerDependencies dependencies) : base(dependencies)
        {
        }
        
        public async Task<IActionResult> Get()
        {
            var results = await DatabaseContext.Users.Where(t => false).ToListAsync();
            return Ok("Everything seems to be operational, database test result should equal 0: " + results.Count);
        }

       
    }
}