using System.Threading.Tasks;
using CSLabs.Api.Services;

namespace CSLabs.Api.Jobs
{
    public class VmStatusJob : AsyncJob
    {
        private readonly TestVmConnectionService _service;
        public VmStatusJob(TestVmConnectionService service) =>
            _service = service;
        protected override async Task ExecuteAsync() => await _service.TestLabVmConnection();
    }
}