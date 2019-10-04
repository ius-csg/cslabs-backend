using CSLabsBackend.Models;
using CSLabsBackend.RequestModels;
using CSLabsBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSLabsBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: BaseController
    {
        
        private readonly IAuthenticationService _authenticationService;

        public UserController(IAuthenticationService authenticationService, DefaultContext defaultContext): base(defaultContext)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateRequest userParam)
        {
            
            var user = _authenticationService.Authenticate(userParam.Email, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] User userParam)
        {
            var test = "";
            return Ok("");
        }

        [HttpGet("current")]
        public IActionResult Current()
        {
            return Ok(GetUser());
        }
    }
}