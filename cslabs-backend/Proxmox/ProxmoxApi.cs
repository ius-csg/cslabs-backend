using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corsinvest.ProxmoxVE.Api;

namespace CSLabsBackend.Proxmox
{
    public class ProxmoxApi
    {
        private PveClient client;
        private bool loggedIn = false;
        private string username;
        private string password;

        public ProxmoxApi(string host, string username,  string password)
        {
            client = new PveClient(host);
            this.username = username;
            this.password = password;
        }

        protected async Task loginIfNotLoggedIn()
        {
            if (!loggedIn) {
                await Task.Run(() =>
                {
                    loggedIn = client.Login(username, password);
                    if (!loggedIn)
                    {
                        throw new UnauthorizedProxmoxUser();
                    }
                });
            }
        }

        public async Task<TicketResponse> GetTicket(string node, int vmId)
        {
            await loginIfNotLoggedIn();
            var output = await Task.Run(() => this.client.Nodes[node].Qemu[vmId].Vncproxy.Vncproxy(websocket: true));

            return new TicketResponse()
            {
                Port = int.Parse(output.Response.data.port),
                Ticket = output.Response.data.ticket,
            };
        }
        
        public async Task StartVM(string node, int vmId)
        {
            await loginIfNotLoggedIn();
            await Task.Run(() => this.client.Nodes[node].Qemu[vmId].Status.Start.VmStart());
        }
        
        
        public async Task StopVM(string node, int vmId)
        {
            await loginIfNotLoggedIn();
            await Task.Run(() => this.client.Nodes[node].Qemu[vmId].Status.Stop.VmStop());
        }

        public async Task ResetVM(string node, int vmId)
        {
            await loginIfNotLoggedIn();
            await Task.Run(() => this.client.Nodes[node].Qemu[vmId].Status.Reset.VmReset());
        }
        
        public async Task ShutdownVm(string node, int vmId)
        {
            await loginIfNotLoggedIn();
            await Task.Run(() => this.client.Nodes[node].Qemu[vmId].Status.Shutdown.VmShutdown());
        }
        
        public async Task<int> CloneTemplate(string node, int vmId)
        {
            await loginIfNotLoggedIn();
            var vmListResponse = await Task.Run(() => this.client.Nodes[node].Qemu.Vmlist());
            var data = (List<object>)vmListResponse.Response.data;
            List<int> ids = new List<int>();
            foreach (IDictionary<string, object> item in data) {
                ids.Add(int.Parse((string)item["vmid"]));
            }

            int newVmId = ids.Max() + 1;
            Console.WriteLine("VmId: " + vmId);
            await Task.Run(() => this.client.Nodes[node].Qemu[vmId].Clone.CloneVm(newVmId));
            
            return newVmId;
        }

        public async Task<VmStatus> GetVmStatus(string node, int vmId)
        {
            await loginIfNotLoggedIn();
            var statusResponse = await Task.Run(() => this.client.Nodes[node].Qemu[vmId].Status.Current.VmStatus());
            var lockValue = "";
            if(((IDictionary<String, object>)statusResponse.Response.data).ContainsKey("lock")) {
                lockValue = statusResponse.Response.data.@lock;
            }
            return new VmStatus
            {
                Lock = lockValue,
                Status = statusResponse.Response.data.status
            };
        }
    }
}