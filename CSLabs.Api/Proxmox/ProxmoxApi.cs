using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corsinvest.ProxmoxVE.Api;
using CSLabs.Api.Models;
using CSLabs.Api.Proxmox.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CSLabs.Api.Proxmox
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

        public async Task<NodeStatus> GetNodeStatus(HypervisorNode node = null)
        {
            if (node == null)
            {
                node = HypervisorNode;
            }
            await LoginIfNotLoggedIn();
            var result =  await Task.Run(() => this.client.Nodes[node.Name].Status.Status());
            var data = result.Response.data;
            var nodeStatus = new NodeStatus
            {
                CpuUsage = data.cpu * 100,
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
        
        
        public async Task ResetVM(int vmId)
        {
            await LoginIfNotLoggedIn();
            await Task.Run(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Status.Reset.VmReset());
        }
        
        public async Task ShutdownVm(int vmId, int timeout = 20)
        {
            await LoginIfNotLoggedIn();
            await Task.Run(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Status.Shutdown.VmShutdown(timeout: timeout));
        }
        
        public async Task DestroyVm(int vmId)
        {
            await LoginIfNotLoggedIn();
            await StopVM(vmId);
            var status = await GetVmStatus(vmId);
            while (!status.IsStopped())
            {
                status = await GetVmStatus(vmId);
            }
            await Task.Run(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].DestroyVm());
        }
        
        public async Task CloneTemplate(HypervisorNode node, int templateId, int vmId)
        {
            await LoginIfNotLoggedIn();
            var response = await Task.Run(() => client.Nodes[HypervisorNode.Name].Qemu[templateId].Clone.CloneVm(vmId, target: node.Name));
            if (!response.IsSuccessStatusCode)
                throw new ProxmoxException("Could not clone template due to: " + response.ReasonPhrase);
        }

        private async Task<List<string>> GetNodes()
        {
            var nodeList = await Task.Run(() => this.client.Nodes.Index());
            var nodes = new List<string>();
            foreach (IDictionary<string, object> item in nodeList.Response.data) {
                nodes.Add((string)item["node"]);
            }

            return nodes;
        }

        private async Task<List<int>> GetVmIds(string node)
        {
            await LoginIfNotLoggedIn();
            var vmListResponse = await Task.Run(() => this.client.Nodes[node].Qemu.Vmlist());
            var data = (List<object>)vmListResponse.Response.data;
            List<int> ids = new List<int>();
            foreach (IDictionary<string, object> item in data) {
                ids.Add(int.Parse((string)item["vmid"]));
            }

            return ids;
        }

        public async Task<List<int>> GetAllUsedIds()
        {
            var nodes = await GetNodes();
            List<int> ids = new List<int>();
            foreach (var node in nodes)
            {
                ids.AddRange((await GetVmIds(node)).Concat(await GetContainerIds(node)));
            }

            return ids;
        }
        
        private async Task<List<int>> GetContainerIds(string node)
        {
            await LoginIfNotLoggedIn();
            var vmListResponse = await Task.Run(() => this.client.Nodes[node].Lxc.Vmlist());
            var data = (List<object>)vmListResponse.Response.data;
            List<int> ids = new List<int>();
            foreach (IDictionary<string, object> item in data) {
                ids.Add(int.Parse((string)item["vmid"]));
            }

            return ids;
        }
        public async Task<int> CloneTemplate(HypervisorNode node, int vmId)
        {
            await LoginIfNotLoggedIn();
            var ids = await GetAllUsedIds();

            int newVmId = 100;
            if(ids.Count != 0)
                newVmId = ids.Max() + 1;
            
            Console.WriteLine("VmId: " + vmId);
            await CloneTemplate(node, vmId, newVmId);
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