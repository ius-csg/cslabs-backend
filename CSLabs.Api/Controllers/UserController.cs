﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.RequestModels;
using CSLabs.Api.Services;
using CSLabs.Api.Email;
using CSLabs.Api.ResponseModels;
using CSLabs.Api.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            user.EmailVerificationCode = Guid.NewGuid().ToString();
            await CreateEmail().SendEmailVerification(user.Email, 
                WebAppUrl + "/verify-email/" + user.EmailVerificationCode);
            
            await _authenticationService.AddUser(user);
            return Ok(_authenticationService.Authenticate(registration.Email, registration.Password));
        }

        [HttpGet("current")]
        public IActionResult Current()
        {
            return Ok(GetUser());
        }

        [HttpGet]
        public IActionResult GetUserList()
        {
            return Ok(
                DatabaseContext.Users
                    .Select(u => new UserViewModel(u))
                    .ToList()
                );
        }

        [AllowAnonymous]
        [HttpPost("forgot-password/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await DatabaseContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
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
            await CreateEmail().SendForgotPasswordEmail(user.Email, link);

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
                .Where(u => u.EmailVerificationCode == request.Code)
                .FirstOrDefaultAsync();
            if (user == null)
                return BadRequest(400);
            user.EmailVerificationCode = null;
            await DatabaseContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("verify-user")]
        public async Task<IActionResult> CheckUserVerification()
        {
            var user = GetUser();
            return Ok(user != null && user.Verified);
        }
        
        [HttpPost("resend-emailverification")]
        public async Task<IActionResult> ResendEmail()
        {
            var user = GetUser();
            user.EmailVerificationCode = Guid.NewGuid().ToString();
            await CreateEmail().SendEmailVerification(user.Email, 
                WebAppUrl + "/verify-email/" + user.EmailVerificationCode);
            await DatabaseContext.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (!request.ValidatePasswordStrength())
            {
                return BadRequest(new GenericErrorResponse
                {
                    Message = "The provided password is not strong enough"
                });
            }
            var user = GetUser();
            var newHashedPassword = this._authenticationService.HashPassword(request.NewPassword);
            var hasher = new PasswordHasher<User>();
            if(hasher.VerifyHashedPassword(user, user.Password, request.CurrentPassword) == PasswordVerificationResult.Failed) {
                return BadRequest(new GenericErrorResponse
                {
                    Message = "Wrong Password, Please try again!"
                });
            }
            user.Password = newHashedPassword;
            await DatabaseContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("change-role")]
        public async Task<IActionResult> ChangeRole(List<ChangeRoleRequest> request)
        {
            if (GetUser().Role != EUserRole.Admin)
            {
                return Unauthorized();
            }
            
            foreach (var requestItem in request)
            {
                var user = await DatabaseContext.Users.FirstOrDefaultAsync(u => u.Id == requestItem.UserId);
                user.Role = requestItem.NewRole;
            }
            await DatabaseContext.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpPut("change-newsletter-subscription")]
        public async Task<IActionResult> ChangeNewsletterSubscription(ChangeNewsletterSubscriptionRequest request)
        {
            var user = GetUser();
            if (user == null)
                return BadRequest(new { message = "Unable to change email subscription" });
            
            user.SubscribedNewsletter = request.Subscribe;
            await DatabaseContext.SaveChangesAsync();
            return Ok();
        }
    }
}