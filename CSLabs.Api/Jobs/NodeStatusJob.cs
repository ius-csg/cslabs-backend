using System;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Controllers;
using CSLabs.Api.Models;
using CSLabs.Api.Proxmox;
using CSLabs.Api.Services;
using FluentScheduler;
using Microsoft.EntityFrameworkCore;
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

        // public void Execute()
        // {
        //     ExecuteAsync().Wait();
        // }

        protected override async Task ExecuteAsync()
        {
            using var scope = _provider.CreateScope();
            using var context = scope.ServiceProvider.GetService<DefaultContext>();
            
            // Do job
            var connectionService = new TestProxmoxConnectionService(context);
            await connectionService.TestProxmoxConnection();
        }
    }
}