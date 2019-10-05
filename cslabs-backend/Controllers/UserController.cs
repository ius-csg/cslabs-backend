using AutoMapper;
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

        public UserController(IAuthenticationService authenticationService, DefaultContext defaultContext, IMapper mapper): base(defaultContext, mapper)
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
        public IActionResult Register([FromBody] RegistrationRequest registration)
        {
            if (!registration.IsValid(DatabaseContext))
                return BadRequest(registration.Validate(DatabaseContext));
            
            var user = Map<User>(registration);
            DatabaseContext.Users.Add(user);
            DatabaseContext.SaveChanges();
            return Ok(_authenticationService.Authenticate(registration.GetEmail(), registration.Password));
        }

        [HttpGet("current")]
        public IActionResult Current()
        {
            return Ok(GetUser());
        }
    }
}