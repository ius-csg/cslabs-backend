using System;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Services;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api.Jobs
{
    public class ApiTokenJob : AsyncJob
    {
        private readonly IServiceProvider _serviceProvider;
        
        public ApiTokenJob(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        protected override async Task ExecuteAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<DefaultContext>();
            var service = scope.ServiceProvider.GetService<ProxmoxApiTokenService>();

            await service.ManageApiToken(context);
        }
    }
}