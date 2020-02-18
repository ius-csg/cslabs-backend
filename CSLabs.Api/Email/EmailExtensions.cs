using System.Collections.Generic;
using System.Threading.Tasks;
using CSLabs.Api.Email.ViewModels;
using CSLabs.Api.RequestModels;
using CSLabs.Api.Util;
using FluentEmail.Core;
using FluentEmail.Core.Models;

namespace CSLabs.Api.Email
{
    public static class EmailExtensions
    {
        public static async Task SendEmailVerification(this IFluentEmail email, string to,  string verificationLink)
        {
            await email
                .To(to)
                .Subject("Please Verify your email Address")
                .UsingTemplateFile("VerifyEmail.cshtml", new VerifyEmailViewModel
                {
                    VerificationLink = verificationLink
                })
                .SendAsync();
        }

        public static async Task SendForgotPasswordEmail(this IFluentEmail email, string to, string forgotPasswordLink)
        {
            var subject = "Forgot Password Confirmation";
            await email
                .To(to)
                .Subject(subject)
                .UsingTemplateFile("ForgotPasswordEmail.cshtml", new ForgotPasswordEmailViewModel()
                {
                    Subject = subject,
                    ForgotPasswordLink = forgotPasswordLink
                })
                .SendAsync();
        }
        
        public static async Task SendNewContactRequestEmail(this IFluentEmail email, List<Address> tosAddresses, ContactUsRequest request )
        {
            var subject = $"CSLabs - New contact request from {request.Email}";
            await email
                .To(tosAddresses)
                .Subject(subject)
                //.AttachFromFilename(contactRequest.UserScreenshot)//possible image attachment
                .UsingTemplateFile("ContactRequest.cshtml", new ContactRequestViewModel
                {
                    Body = request.Message,
                    Email = request.Email,
                    Subject = subject
                })
                .SendAsync();
        }
    }
}