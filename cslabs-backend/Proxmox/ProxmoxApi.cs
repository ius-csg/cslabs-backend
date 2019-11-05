using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Rundeck;
using  RundeckClient = Rundeck.Client;
namespace CSLabsBackend.Proxmox
{
    public class ProxmoxApi
    {
        private RundeckClient client;
        
        public ProxmoxApi(string scheme, string host, string authToken)
        {
            this.client = new RundeckClient(scheme, host, authToken);
        }


        public ExecutionParams getVmIdParams(int vmId)
        {
            return new ExecutionParams() {options = new Dictionary<string, string>() {
                ["VmId"] = vmId.ToString()
            }};
        }

        public async Task<TicketResponse> GetTicket(int vmId)
        {
            var output = await this.client.RunJobAndGetOutput("548949b7-938a-4edf-b13e-7551e8992144", getVmIdParams(vmId));
            return JsonConvert.DeserializeObject<TicketResponse>(output);
        }
        
        public async Task StartVM(int vmId)
        {
            await this.client.RunJobAndGetOutput("70a524ac-bd20-4703-8960-543fb9e77a14", getVmIdParams(vmId));
        }
        
        
        public async Task StopVM(int vmId)
        {
            await this.client.RunJobAndGetOutput("47e66657-185a-4cb7-967e-9c65b05d3613", getVmIdParams(vmId));
        }
        
        
        public async Task ShutdownVm(int vmId)
        {
            await this.client.RunJobAndGetOutput("584b39ea-02a4-48e0-bcfb-bee0252889f6", getVmIdParams(vmId));
        }
        
        public async Task<int> CloneTemplate(int vmId)
        {
            var output = await this.client.RunJobAndGetOutput("41aeae37-eb19-4081-8cf2-c9147a8ac004", getVmIdParams(vmId));
            return int.Parse(output);
        }
    }
}