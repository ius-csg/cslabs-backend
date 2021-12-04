using System;
using FluentScheduler;

namespace CSLabs.Api.Jobs
{
    public class JobRegistry : Registry
    {
        public JobRegistry(IServiceProvider provider)
        {
            Schedule(() => new MyJob(provider)).ToRunNow().AndEvery(2).Seconds();
        }
    }
}