using System;
using FluentScheduler;

namespace CSLabs.Api.Jobs
{
    public class JobRegistry : Registry

    {
        public JobRegistry(IServiceProvider provider)
        {
            // Every minute check the status of each Lab VM within each Lab
            // Every 3 minutes check to see if we have quorum (at least 50% of the ProxMox nodes are up)
            
            // Schedule new jobs here
            Schedule(() => new VmStatusJob(provider)).ToRunEvery(1).Minutes();
            Schedule(() => new NodeStatusJob(provider)).ToRunEvery(3).Minutes();

        }
    }
}