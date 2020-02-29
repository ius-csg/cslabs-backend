using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Config;
using CSLabs.Api.Models;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Proxmox;

namespace CSLabs.Api.Services
{
    public class UserLabInstantiationService
    {
        private readonly DefaultContext _context;
        
        public UserLabInstantiationService(DefaultContext context)
        {
            _context = context;
        }
        
        private HypervisorNode GetFirstAvailableHypervisorNodeFromTemplates(List<LabVm> labVms)
        {
            return labVms.SelectMany(vm => vm.VmTemplates).Select(t => t.HypervisorNode).First();
        }

        public async Task Instantiate(UserLab userLab, ProxmoxManager ProxmoxManager)
        {
            var node = await ProxmoxManager.GetLeastLoadedHyperVisorNode(userLab.Lab);
            var api = ProxmoxManager.GetProxmoxApi(GetFirstAvailableHypervisorNodeFromTemplates(userLab.Lab.LabVms));
            List<UserLabVm> vms = new List<UserLabVm>();

            var lanBridge = new HypervisorBridgeInstance {InterfaceId = await api.CreateBridge()};

            userLab.BridgeInstances = new List<HypervisorBridgeInstance> {lanBridge};
            
            foreach (var labVm in userLab.Lab.LabVms)
            {
                var template = labVm.GetTemplateWithNode(api.HypervisorNode);
                int createdVmId = await api.CloneTemplate(node, template.TemplateVmId);
                vms.Add(new UserLabVm
                {
                    LabVm = labVm,
                    ProxmoxVmId = createdVmId,
                    VmTemplate = template,
                    IsCoreRouter = template.IsCoreRouter,
                    BridgeInstances = new List<HypervisorBridgeInstance> {lanBridge}
                });
            }

            foreach (var vm in vms)
            {
                await api.AddBridgeToVm(vm.ProxmoxVmId, lanBridge.InterfaceId, vm.IsCoreRouter ? 1 : 0, node.Name);
            }

            await api.ApplyNetworkConfiguration();
            
            foreach (var vm in vms)
            {
                await api.StartVM(vm.ProxmoxVmId, node.Name);
            }
            
            userLab.HypervisorNode = node;
            userLab.UserLabVms = vms;
            userLab.EndDateTime = DateTime.UtcNow.AddDays(30);
            userLab.Status = EUserLabStatus.Started;
        }

        public async Task Deconstruct(UserLab userLab, ProxmoxManager proxmoxManager)
        {
            var api = proxmoxManager.GetProxmoxApi(userLab);
            foreach (var userLabVm in userLab.UserLabVms)
            {
                await api.DestroyVm(userLabVm.ProxmoxVmId);
                _context.UserLabVms.Remove(userLabVm);
                await _context.SaveChangesAsync();
            }
            
            foreach (var bridge in userLab.BridgeInstances)
            {
                await api.DestroyBridge(bridge.InterfaceId);
                _context.HypervisorBridgeInstances.Remove(bridge);
                await _context.SaveChangesAsync();
            }

            userLab.Status = EUserLabStatus.Completed;
        }
    }
}