using System;
using System.Threading.Tasks;
using CommandLine;
using CSLabs.Api.Models;
using CSLabs.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.ConsoleUtil.Commands
{
    [Verb("user:change-password", HelpText = "Changes the password for a user")]
    public class ChangeUserPasswordOptions
    {
        [Option(Required = true, HelpText = "The email of the user")]
        public string Email {get; set; }
        [Option(Required = true, HelpText = "The new password")]
        public string Password { get; set; }
    }
    public class ChangeUserPasswordCommand : AsyncCommand<ChangeUserPasswordOptions>
    {
        private DefaultContext _context;
        private IAuthenticationService _authenticationService;
        
        public ChangeUserPasswordCommand(DefaultContext context, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _context = context;
        }
        
        public override async Task Run(ChangeUserPasswordOptions options)
        {
            var email = options.Email.Trim();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                Console.WriteLine($"User not found with email: {email}");
                return;
            }
            user.Password = _authenticationService.HashPassword(options.Password);
            await _context.SaveChangesAsync();
            Console.WriteLine("Password successfully changed!");
        }
    }
}