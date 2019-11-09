using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSLabsBackend.Config;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Rundeck;
using  RundeckClient = Rundeck.Client;
namespace CSLabsBackend.Proxmox
{
    public class ProxmoxApi
    {
        private RundeckClient client;
        private RundeckJobIdSettings jobIds;
        public ProxmoxApi(string scheme, string host, string apiKey, RundeckJobIdSettings jobIds)
        {
            this.client = new RundeckClient(scheme, host, apiKey);
            this.jobIds = jobIds;
        }


        public ExecutionParams getVmIdParams(int vmId)
        {
            return new ExecutionParams() {options = new Dictionary<string, string>() {
                ["VmId"] = vmId.ToString()
            }};
        }

        public async Task<TicketResponse> GetTicket(int vmId)
        {
            var output = await this.client.RunJobAndGetOutput(jobIds.GetTicket, getVmIdParams(vmId));
            return JsonConvert.DeserializeObject<TicketResponse>(output);
        }
        
        public async Task StartVM(int vmId)
        {
            await this.client.RunJobAndGetOutput(jobIds.StartVm, getVmIdParams(vmId));
        }
        
        
        public async Task StopVM(int vmId)
        {
            await this.client.RunJobAndGetOutput(jobIds.StopVm, getVmIdParams(vmId));
        }
        
        
        public async Task ShutdownVm(int vmId)
        {
            await this.client.RunJobAndGetOutput(jobIds.ShutdownVm, getVmIdParams(vmId));
        }
        
        public async Task<int> CloneTemplate(int vmId)
        {
            Console.WriteLine("VmId: " + vmId);
            var output = await this.client.RunJobAndGetOutput(jobIds.CloneTemplate, getVmIdParams(vmId));
            return int.Parse(output);
        }

        public async Task<VmStatus> GetVmStatus(int vmId)
        {
            var output = await this.client.RunJobAndGetOutput(jobIds.GetVmStatus, getVmIdParams(vmId));
            return JsonConvert.DeserializeObject<VmStatus>(output);
        }
    }
}