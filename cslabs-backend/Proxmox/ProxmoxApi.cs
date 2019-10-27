using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<TicketResponse> GetTicket(int vmId)
        {
            var output = await this.client.RunJobAndGetOutput("548949b7-938a-4edf-b13e-7551e8992144",
                new ExecutionParams()
                {
                    options = new Dictionary<string, string>()
                    {
                        ["VmId"] = vmId.ToString()
                    }
                });
            return JsonConvert.DeserializeObject<TicketResponse>(output);
        }
    }
}