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
            var output = await this.client.RunJobAndGetOutput("***REMOVED***", getVmIdParams(vmId));
            return JsonConvert.DeserializeObject<TicketResponse>(output);
        }
        
        public async Task StartVM(int vmId)
        {
            await this.client.RunJobAndGetOutput("***REMOVED***", getVmIdParams(vmId));
        }
        
        
        public async Task StopVM(int vmId)
        {
            await this.client.RunJobAndGetOutput("***REMOVED***", getVmIdParams(vmId));
        }
        
        
        public async Task ShutdownVm(int vmId)
        {
            await this.client.RunJobAndGetOutput("***REMOVED***", getVmIdParams(vmId));
        }
        
        public async Task<int> CloneTemplate(int vmId)
        {
            var output = await this.client.RunJobAndGetOutput("***REMOVED***", getVmIdParams(vmId));
            return int.Parse(output);
        }
    }
}