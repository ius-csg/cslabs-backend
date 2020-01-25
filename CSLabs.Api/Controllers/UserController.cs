using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.RequestModels;
using CSLabs.Api.Services;
using CSLabs.Api.Email;
using CSLabs.Api.Email.ViewModels;
using CSLabs.Api.Util;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
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
            try
            {
                var user = _authenticationService.Authenticate(userParam.Email, userParam.Password);
                return Ok(user);
            }
            catch (AuthenticationFailureException)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

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
        
        [AllowAnonymous]
        [HttpPost("forgot-password/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await DatabaseContext.Users
                .FirstOrDefaultAsync(u => u.SchoolEmail == email || u.PersonalEmail == email);
            if (user == null)
            {
                // make sure bots can't tell if a user is found or not.
                await Task.Delay(new Random().Next(2000, 2500));
                return Ok();
            }
               
            var uuid = Guid.NewGuid().ToString();
            user.PasswordRecoveryCode = uuid;
            var link = WebAppUrl + "/confirm-forgot-password/" + uuid;
            await DatabaseContext.SaveChangesAsync();
            if (user.SchoolEmail != null)
                await CreateEmail().SendForgotPasswordEmail(user.SchoolEmail, link);
            
            if (user.PersonalEmail != null)
                await CreateEmail().SendForgotPasswordEmail(user.PersonalEmail, link);

            return Ok();
        }
        
        [AllowAnonymous]
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