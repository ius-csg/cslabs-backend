using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSLabs.Api.Models;
using CSLabs.Api.Services;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemMessageController : BaseController
    {
        private SystemMessageService Service { get; }
        public SystemMessageController(BaseControllerDependencies deps) : base(deps)
        {
            Service = new SystemMessageService(deps.DatabaseContext, new TimeService());
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemMessage>>> GetSystemMessages()
        {
            return await Service.GetActiveMessages();
        }
    }
}
