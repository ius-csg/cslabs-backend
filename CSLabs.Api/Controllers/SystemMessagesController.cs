using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
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

    public class SystemMessagesController : BaseController
    {
        public SystemMessagesController(BaseControllerDependencies dependencies) : base(dependencies)
        {


            [HttpGet("{id}")] 
            public async Task<IActionResult> GetSystemMessageID(int id)
            {
                return; 
            }

            [HttpPost] //Create
            public async Task<IActionResult> UpsertSystemMessage()
            {
                return;
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteSystemMessage(int id)
            {
                return; 
            }
            
            [HttpPut("{id}")] //Update
            public async Task<IActionResult> UpdateSystemMessage(int id)
            {
                return; 
            }
            
        }
    }
}
