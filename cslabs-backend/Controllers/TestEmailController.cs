using System.IO;
using System.Threading.Tasks;
using CSLabsBackend.EmailViewModels;
using CSLabsBackend.Util;
using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;

namespace CSLabsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestEmailController : BaseController
    {
        public TestEmailController(BaseControllerDependencies dependencies) : base(dependencies)
        {
        }

        [HttpPost("")]
        public async Task<IActionResult> Post()
        {
            await CreateEmail()
                .To("jasongallavin@gmail.com")
                .Subject("Test Email")
                .UsingTemplateFile("TestEmail.cshtml", new TestEmailViewModel{Message = "Test Email", Subject = "Test Email"})
                .SendAsync();
            return Ok();
        }
    }
}