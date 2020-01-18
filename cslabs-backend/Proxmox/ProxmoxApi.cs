using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corsinvest.ProxmoxVE.Api;
using CSLabsBackend.Models;
using CSLabsBackend.Proxmox.Responses;

namespace CSLabsBackend.Proxmox
{
    public class ProxmoxApi
    {
        private PveClient client;
        private DateTime _loggedInAt = DateTime.MinValue;
        private string _password;
        public HypervisorNode HypervisorNode { get;}
        public ProxmoxApi(HypervisorNode hypervisorNode, string password)
        {
            client = new PveClient(hypervisorNode.Hypervisor.Host);
            HypervisorNode = hypervisorNode;
            _password = password;
        }

        private bool loggedIn => DateTime.Now.Subtract(_loggedInAt).TotalMinutes < 15;

        private async Task LoginIfNotLoggedIn()
        {
            if (!loggedIn) await Task.Run(Login);
        }

        private void Login()
        {
            bool successful = client.Login(HypervisorNode.Hypervisor.UserName, _password);
            if (!successful)
                throw new UnauthorizedProxmoxUser();
            _loggedInAt = DateTime.Now;
        }

        public async Task<TicketResponse> GetTicket(int vmId)
        {
            await LoginIfNotLoggedIn();
            var output = await Task.Run(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Vncproxy.Vncproxy(websocket: true));

            return new TicketResponse()
            {
                Port = int.Parse(output.Response.data.port),
                Ticket = output.Response.data.ticket,
            };
        }
        
        public async Task StartVM(int vmId)
        {
            await LoginIfNotLoggedIn();
            await Task.Run(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Status.Start.VmStart());
        }

        public async Task<NodeStatus> GetNodeStatus()
        {
            await LoginIfNotLoggedIn();
            var result =  await Task.Run(() => this.client.Nodes[HypervisorNode.Name].Status.Status());
            var data = result.Response.data;
            var nodeStatus = new NodeStatus
            {
                CpuUsage = data.cpu,
                MemoryUsage = new MemoryUsage
                {
                    Free = data.memory.free,
                    Used = data.memory.used,
                    Total = data.memory.total
                }
            };
            return nodeStatus;

        }
        
        
        public async Task StopVM(int vmId)
        {
            await LoginIfNotLoggedIn();
            await Task.Run(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Status.Stop.VmStop());
        }
        
        
        public async Task ShutdownVm(int vmId)
        {
            await LoginIfNotLoggedIn();
            await Task.Run(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Status.Shutdown.VmShutdown());
        }
        
        public async Task<int> CloneTemplate(HypervisorNode node, int vmId)
        {
            await LoginIfNotLoggedIn();
            var vmListResponse = await Task.Run(() => this.client.Nodes[node.Name].Qemu.Vmlist());
            var data = (List<object>)vmListResponse.Response.data;
            List<int> ids = new List<int>();
            foreach (IDictionary<string, object> item in data) {
                ids.Add(int.Parse((string)item["vmid"]));
            }

            int newVmId = ids.Max() + 1;
            Console.WriteLine("VmId: " + vmId);
            await Task.Run(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Clone.CloneVm(newVmId, target: HypervisorNode.Name));
            
            return newVmId;
        }

        public async Task<VmStatus> GetVmStatus(int vmId)
        {
            await LoginIfNotLoggedIn();
            var statusResponse = await Task.Run(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Status.Current.VmStatus());
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