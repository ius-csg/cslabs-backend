using System.Threading.Tasks;
using FluentScheduler;

namespace CSLabs.Api.Jobs
{
    public abstract class AsyncJob : IJob
    {
        public void Execute()
        {
            ExecuteAsync().Wait();
        }

        protected abstract Task ExecuteAsync();
    }
}