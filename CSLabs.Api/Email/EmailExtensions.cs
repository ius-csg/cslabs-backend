using System.Threading.Tasks;
using CSLabs.Api.Email.ViewModels;
using CSLabs.Api.Util;
using FluentEmail.Core;

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
    }
}