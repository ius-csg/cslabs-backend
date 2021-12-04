using System;
using CSLabs.Api.Models;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api.Jobs
{
    public class MyJob : IJob
    {
        private IServiceProvider _provider;
        public MyJob(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Execute()
        {
            using var scope = _provider.CreateScope();
            using var context = scope.ServiceProvider.GetService<DefaultContext>();
            Console.WriteLine("Job Ran!");
        }
    }
}