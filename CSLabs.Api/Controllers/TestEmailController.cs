using System.IO;
using System.Threading.Tasks;
using CSLabs.Api.Email.ViewModels;
using CSLabs.Api.Util;
using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;

namespace CSLabs.Api.Controllers
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
                .To("test@test.com")
                .Subject("Test Email")
                .UsingTemplateFile("TestEmail.cshtml", new TestEmailViewModel{Message = "Test Email", Subject = "Test Email"})
                .SendAsync();
            return Ok();
        }
    }
}