using System;
using CSLabs.Api.Models;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api.Jobs
{
    public class ExampleJob : IJob
    {
        private readonly IServiceProvider _provider;
        public ExampleJob(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void Execute()
        {
            using var scope = _provider.CreateScope();
            using var context = scope.ServiceProvider.GetService<DefaultContext>();
            
            // Do job
            Console.WriteLine("Job Ran!");
        }
    }
}