using System;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api.Jobs
{
    public class JobRegistry : Registry

    {
        public JobRegistry(IServiceProvider provider)
        {
            // Every minute check the status of each Lab VM within each Lab
            // Every 3 minutes check to see if we have quorum (at least 50% of the ProxMox nodes are up)
            
            // Schedule new jobs here
            Schedule(provider.GetService<InjectedAsyncJob<VmStatusJob>>).ToRunEvery(1).Minutes();
        }
    }
}