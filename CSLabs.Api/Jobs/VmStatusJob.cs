using System;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api.Jobs
{
    public class VmStatusJob : AsyncJob
    {
        private readonly IServiceProvider _provider;
        public VmStatusJob(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override async Task ExecuteAsync()
        {
            using var scope = _provider.CreateScope();
            var connectionService = scope.ServiceProvider.GetService<TestVmConnectionService>();
            await connectionService.TestLabVmConnection();

        }
    }
}