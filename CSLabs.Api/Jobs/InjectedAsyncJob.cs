using System;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api.Jobs
{
    public class InjectedAsyncJob<T> : IJob where T : AsyncJob
    {
        private IServiceProvider _provider;

        public InjectedAsyncJob(IServiceProvider provider) => _provider = provider;

        public void Execute()
        {
            using var scope = _provider.CreateScope();
            scope.ServiceProvider.GetService<T>().Execute();
        }
    }
}