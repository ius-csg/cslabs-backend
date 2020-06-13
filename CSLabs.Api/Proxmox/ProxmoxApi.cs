using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Corsinvest.ProxmoxVE.Api;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Proxmox.Responses;
using Newtonsoft.Json;

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
            if (!loggedIn) await Login();
        }

        private async Task Login()
        {
            bool successful = await Task.Run(() => client.Login(HypervisorNode.Hypervisor.UserName, _password));
            if (!successful)
                throw new UnauthorizedProxmoxUser();
            _loggedInAt = DateTime.Now;
        }

        public async Task<TicketResponse> GetTicket(int vmId)
        {
            await LoginIfNotLoggedIn();
            var output = await PerformRequest(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Vncproxy.Vncproxy(websocket: true));

            return new TicketResponse()
            {
                Port = int.Parse(output.Response.data.port),
                Ticket = output.Response.data.ticket,
            };
        }
        
        public async Task StartVM(int vmId, string targetNode = null)
        {
            var node = targetNode ?? HypervisorNode.Name;
            await LoginIfNotLoggedIn();
            await PerformRequest(() => this.client.Nodes[node].Qemu[vmId].Status.Start.VmStart());
        }

        public async Task<NodeStatus> GetNodeStatus(HypervisorNode node = null)
        {
            if (node == null)
            {
                node = HypervisorNode;
            }
            await LoginIfNotLoggedIn();
            var response = await PerformRequest(() => this.client.Nodes[node.Name].Status.Status());
            var data = response.Response.data;
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
            await PerformRequest(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Status.Stop.VmStop());
        }

        public async Task ResetVM(int vmId)
        {
            await LoginIfNotLoggedIn();
            await PerformRequest(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Status.Reset.VmReset());
        }
        
        public async Task ShutdownVm(int vmId, int timeout = 20)
        {
            await LoginIfNotLoggedIn();
            await PerformRequest(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Status.Shutdown.VmShutdown(timeout: timeout));
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
            await PerformRequest(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].DestroyVm());
        }
        
        public async Task CloneTemplate(HypervisorNode node, int templateId, int vmId, string srcNode = null)
        {
            var sourceNode = srcNode ?? HypervisorNode.Name;
            await LoginIfNotLoggedIn();
            await PerformRequest(() => client.Nodes[sourceNode].Qemu[templateId].Clone.CloneVm(vmId, target: node.Name));
        }

        private async Task<List<string>> GetNodes()
        {
            var nodeList = await PerformRequest(() => this.client.Nodes.Index());
            var nodes = new List<string>();
            foreach (IDictionary<string, object> item in nodeList.Response.data) {
                nodes.Add((string)item["node"]);
            }

            return nodes;
        }

        private async Task<List<int>> GetQemuIds(string node)
        {
            await LoginIfNotLoggedIn();
            var vmListResponse = await PerformRequest(() => this.client.Nodes[node].Qemu.Vmlist());
            var data = (List<object>)vmListResponse.Response.data;
            List<int> ids = new List<int>();
            foreach (IDictionary<string, object> item in data) {
                ids.Add(int.Parse((string)item["vmid"]));
            }

            return ids;
        }
        

        public async Task<List<int>> GetAllUsedVmIds()
        {
            var nodes = await GetNodes();
            List<int> ids = new List<int>();
            foreach (var node in nodes)
            {
                ids.AddRange((await GetQemuIds(node)).Concat(await GetContainerIds(node)));
            }

            return ids;
        }
        
        private async Task<List<int>> GetContainerIds(string node)
        {
            await LoginIfNotLoggedIn();
            var vmListResponse = await PerformRequest(() => this.client.Nodes[node].Lxc.Vmlist());
            var data = (List<object>)vmListResponse.Response.data;
            List<int> ids = new List<int>();
            foreach (IDictionary<string, object> item in data) {
                ids.Add(int.Parse((string)item["vmid"]));
            }

            return ids;
        }

        private async Task<int> GetNextAvailableVmId()
        {
            var ids = await GetAllUsedVmIds();
            int newVmId = 100;
            if(ids.Count != 0)
                newVmId = ids.Max() + 1;
            return newVmId;
        }
        
        public async Task<int> CloneTemplate(HypervisorNode node, int vmId, string srcNode = null)
        {
            await LoginIfNotLoggedIn();
            var newVmId = await GetNextAvailableVmId();
            await CloneTemplate(node, vmId, newVmId);
            return newVmId;
        }

        public async Task<VmStatus> GetVmStatus(int vmId)
        {
            await LoginIfNotLoggedIn();
            var statusResponse = await PerformRequest(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Status.Current.VmStatus());
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
        
        public async Task<string> GetVmUnusedDisk(int vmId)
        {
            await LoginIfNotLoggedIn();
            var statusResponse = await PerformRequest(() => this.client.Nodes[HypervisorNode.Name].Qemu[vmId].Config.GetRest());
            var data = statusResponse.Response.data;
            if (((IDictionary<String, object>) data).ContainsKey("unused0"))
            {
                return data.unused0;
            }

            return null;
        }

        public async Task<int> CreateBridge(string targetNode = null)
        {
            await LoginIfNotLoggedIn();
            string node = targetNode ?? this.HypervisorNode.Name;
            var bridgeId = await GetNextAvailableBridgeId();
            var interfaceName = "vmbr" + bridgeId;
            await PerformRequest(() => this.client.Nodes[node].Network.CreateNetwork(iface: interfaceName, type: "bridge", autostart: true));
            return bridgeId;
        }
        
        public async Task<int> CreateVm(string name, int memoryMb, string targetNode = null)
        {
            await LoginIfNotLoggedIn();
            string node = targetNode ?? this.HypervisorNode.Name;
            var vmId = await GetNextAvailableVmId();
            await PerformRequest(() => this.client.Nodes[node].Qemu.CreateVm(vmid: vmId, memory: memoryMb, cores: 1, name: name));
            return vmId;
        }

        public async Task DestroyBridge(int bridgeId, string targetNode = null)
        {
            await LoginIfNotLoggedIn();
            string node = targetNode ?? this.HypervisorNode.Name;
            var interfaceName = "vmbr" + bridgeId;
            await PerformRequest(() => this.client.Nodes[node].Network[interfaceName].DeleteNetwork());
        }

        public async Task ApplyNetworkConfiguration(string targetNode = null)
        {
            await LoginIfNotLoggedIn();
            string node = targetNode ?? this.HypervisorNode.Name;
            await PerformRequest(() => this.client.Nodes[node].Network.ReloadNetworkConfig());
        }

        public async Task AddBridgeToVm(int vmId, int bridgeId, int interfaceNumber = 0, string targetNode = null)
        {
            await LoginIfNotLoggedIn();
            string node = targetNode ?? this.HypervisorNode.Name;
            var dic = new Dictionary<int, string>
            {
                {interfaceNumber, "model=virtio,bridge=vmbr" + bridgeId + ",firewall=1"}
            };
            await PerformRequest(() => this.client.Nodes[node].Qemu[vmId].Config.UpdateVmAsync(netN: dic));
        }
        
        public async Task<ExpandoObject> GetConfig(int vmId, string targetNode = null)
        {
            await LoginIfNotLoggedIn();
            string node = targetNode ?? this.HypervisorNode.Name;
            var result = await PerformRequest(() => this.client.Nodes[node].Qemu[vmId].Config.VmConfig());
            return result.Response.data;
        }
        
        public async Task UpdateBootDisk(int vmId, string targetDisk, string targetNode = null)
        {
            await LoginIfNotLoggedIn();
            string node = targetNode ?? this.HypervisorNode.Name;
            await PerformRequest(() => this.client.Nodes[node].Qemu[vmId].Config.UpdateVmAsync(bootdisk: targetDisk));
        }

        public async Task SetVmScsi0(int vmId, string disk, string targetNode = null)
        {
            await LoginIfNotLoggedIn();
            string node = targetNode ?? this.HypervisorNode.Name;
            // var optionsStr = string.Join(",", options.ToList().Select(pair => pair.Key + "=" + pair.Value));
            var scsiOptions = new Dictionary<int, string> {{0, disk}};
            await PerformRequest(() => this.client.Nodes[node].Qemu[vmId].Config.UpdateVmAsync(scsiN: scsiOptions, bootdisk: "scsi0"));
        }
        
        public async Task ConvertVmToTemplate(int vmId, string targetNode = null)
        {
            await LoginIfNotLoggedIn();
            string node = targetNode ?? this.HypervisorNode.Name;
            await PerformRequest(() => this.client.Nodes[node].Qemu[vmId].Template.CreateRest());
        }

        private async Task<int> GetNextAvailableBridgeId()
        {
            await LoginIfNotLoggedIn();
            var interfaces = await GetBridgeIds();
            int newVmbrId = 10;
            if(interfaces.Count != 0)
                newVmbrId = interfaces.Max() + 1;
            if (newVmbrId < 10)
                newVmbrId = 10;
            return newVmbrId;
        }

        public async Task<List<int>> GetBridgeIds(string targetNode = null)
        {
            await LoginIfNotLoggedIn();
            string node = targetNode ?? this.HypervisorNode.Name;
            var response = await PerformRequest(() => this.client.Nodes[node].Network.Index());
            List<int> bridgeIds = new List<int>();
            foreach (var responseInterface in response.Response.data)
            {
                string name = responseInterface.iface;
                if (name.Contains("vmbr"))
                {
                    string strNum = name.Substring(4);
                    bridgeIds.Add(int.Parse(strNum));
                }
            }
            return bridgeIds;
        }
        
        private async Task<Result> PerformRequest(Func<Result> request, [CallerMemberName] string methodName = "")
        {
            var response = await Task.Run(request);
            if (!response.IsSuccessStatusCode) {
                throw new ProxmoxRequestException(
                    $"{methodName} Failed with response: " + 
                    JsonConvert.SerializeObject(response.Response) + 
                    " with reason: " + response.ReasonPhrase, response
                );
            }

            return response;
        }
    }
}