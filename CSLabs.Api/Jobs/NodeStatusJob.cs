using System;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api.Jobs
{
    public class NodeStatusJob : AsyncJob
    {
        private readonly IServiceProvider _provider;
        public NodeStatusJob(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override async Task ExecuteAsync()
        {
            using var scope = _provider.CreateScope();
            using var context = scope.ServiceProvider.GetService<DefaultContext>();
            
            // Do job
            var connectionService = new TestProxmoxNodeConnectionService(context);
            await connectionService.TestProxmoxConnection();

        }
    }
}