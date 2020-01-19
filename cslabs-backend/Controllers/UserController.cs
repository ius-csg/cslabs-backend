using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSLabsBackend.Email;
using CSLabsBackend.Email.ViewModels;
using CSLabsBackend.Models.UserModels;
using CSLabsBackend.RequestModels;
using CSLabsBackend.Services;
using CSLabsBackend.Util;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: BaseController
    {
        
        private readonly IAuthenticationService _authenticationService;

        public UserController(IAuthenticationService authenticationService, BaseControllerDependencies deps): base(deps)
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
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registration)
        {
            if (!registration.IsValid(DatabaseContext))
                return BadRequest(registration.Validate(DatabaseContext));

            var user = Map<User>(registration);
            user.SetNulls();
            if (user.PersonalEmail != null)
            {
                user.PersonalEmailVerificationCode = Guid.NewGuid().ToString();
                await CreateEmail().SendEmailVerification(user.PersonalEmail,
                    WebAppUrl + "/verify-email/personal/" + user.PersonalEmailVerificationCode);
            }
            else if(user.SchoolEmail != null)
            {
                user.SchoolEmailVerificationCode = Guid.NewGuid().ToString();
                await CreateEmail().SendEmailVerification(user.SchoolEmail,
                    WebAppUrl + "/verify-email/school/" + user.SchoolEmailVerificationCode);
            }
            
            await _authenticationService.AddUser(user);
            return Ok(_authenticationService.Authenticate(registration.GetEmail(), registration.Password));
        }

        [HttpGet("current")]
        public IActionResult Current()
        {
            return Ok(GetUser());
        }

        [HttpPost("forgot-password/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await DatabaseContext.Users
                .FirstOrDefaultAsync(u => u.SchoolEmail == email || u.PersonalEmail == email);
            if (user == null)
                return Ok();
            var uuid = Guid.NewGuid().ToString();
            user.PasswordRecoveryCode = uuid;
            var link = WebAppUrl + "confirm-forgot-password/" + uuid;
            await DatabaseContext.SaveChangesAsync();
            if (user.SchoolEmail != null)
                await CreateEmail().SendForgotPasswordEmail(user.SchoolEmail, link);
            
            if (user.PersonalEmail != null)
                await CreateEmail().SendForgotPasswordEmail(user.PersonalEmail, link);

            return Ok();
        }
        
        [HttpPost("confirm-forgot-password")]
        public async Task<IActionResult> ConfirmForgotPassword([FromBody] ConfirmForgotPasswordRequest request)
        {
            var user = await DatabaseContext.Users
                .FirstOrDefaultAsync(u => u.PasswordRecoveryCode != null && u.PasswordRecoveryCode == request.PasswordRecoveryCode);
            if(user == null)
                return BadRequest("Password recovery code is not correct");
            await _authenticationService.ChangePassword(user, request.NewPassword);
            return Ok();
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationRequest request)
        {
            var user = await DatabaseContext
                .Users
                .Where(u => 
                    request.Type == "school" ? 
                        u.SchoolEmailVerificationCode == request.Code : 
                        u.PersonalEmailVerificationCode == request.Code)
                .FirstOrDefaultAsync();
            if (user == null)
                return BadRequest(400);

            if (request.Type == "school")
                user.SchoolEmailVerificationCode = null;
            else
                user.PersonalEmailVerificationCode = null;
            await DatabaseContext.SaveChangesAsync();
            return Ok();
        }
    }
}