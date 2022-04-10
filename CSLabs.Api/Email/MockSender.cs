using System.Threading;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;

namespace CSLabs.Api.Email
{
    public class MockSender : ISender
    {
        public SendResponse Send(IFluentEmail email, CancellationToken? token = null) =>
            new SendResponse { MessageId = "mocked-email" };
        
        public Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null) =>
            Task.FromResult(new SendResponse { MessageId = "mocked-email" });
    }
}