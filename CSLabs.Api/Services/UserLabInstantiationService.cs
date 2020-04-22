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
            return labVms.SelectMany(vm => vm.VmTemplate.HypervisorVmTemplates).Select(t => t.HypervisorNode).First();
        }

        public async Task LinkVmToBridges(UserLab userLab, UserLabVm vm, ProxmoxApi api, HypervisorNode node)
        {
            foreach (var templateInterface in vm.LabVm.TemplateInterfaces)
            {
                var bridge = userLab.BridgeInstances.First(b => b.BridgeTemplate == templateInterface.BridgeTemplate);
                await api.AddBridgeToVm(vm.ProxmoxVmId, bridge.HypervisorInterfaceId, templateInterface.InterfaceNumber, node.Name);
            }
        }

        public async Task LinkVmToBridgesAndAddInstances(UserLab userLab, UserLabVm vm, ProxmoxApi api, HypervisorNode node)
        {
            await LinkVmToBridges(userLab, vm, api, node);
            foreach (var templateInterface in vm.LabVm.TemplateInterfaces)
            {
                var bridge = userLab.BridgeInstances.First(b => b.BridgeTemplate == templateInterface.BridgeTemplate);
                vm.InterfaceInstances.Add(new VmInterfaceInstance
                {
                    BridgeInstance = bridge,
                    Template = templateInterface
                });
            }
          
        }
        public async Task Instantiate(UserLab userLab, ProxmoxManager ProxmoxManager)
        {
            var node = await ProxmoxManager.GetLeastLoadedHyperVisorNode(userLab.Lab);
            var api = ProxmoxManager.GetProxmoxApi(GetFirstAvailableHypervisorNodeFromTemplates(userLab.Lab.LabVms));
            foreach (var bridgeTemplate in userLab.Lab.BridgeTemplates) {
                userLab.BridgeInstances.Add(new BridgeInstance
                {
                    HypervisorInterfaceId = await api.CreateBridge(), 
                    BridgeTemplate = bridgeTemplate,
                });
            }
            
            foreach (var labVm in userLab.Lab.LabVms)
            {
                var template = labVm.GetTemplateWithNode(api.HypervisorNode);
                int createdVmId = await api.CloneTemplate(node, template.TemplateVmId);
                userLab.UserLabVms.Add(new UserLabVm
                {
                    LabVm = labVm,
                    ProxmoxVmId = createdVmId,
                    HypervisorVmTemplate = template,
                    IsCoreRouter = template.VmTemplate.IsCoreRouter
                });
            }

            foreach (var vm in userLab.UserLabVms)
            {
                await LinkVmToBridgesAndAddInstances(userLab, vm, api, node);
            }

            await api.ApplyNetworkConfiguration();
            
            foreach (var vm in userLab.UserLabVms)
            {
                await api.StartVM(vm.ProxmoxVmId, node.Name);
            }
            
            userLab.HypervisorNode = node;
            userLab.EndDateTime = DateTime.UtcNow.AddDays(30);
            userLab.Status = EUserLabStatus.Started;
        }

        public async Task Deconstruct(UserLab userLab, ProxmoxManager proxmoxManager)
        {
            var api = proxmoxManager.GetProxmoxApi(userLab);
            var userLabVms = userLab.UserLabVms.ToList();
            foreach (var userLabVm in userLabVms)
            {
                await api.DestroyVm(userLabVm.ProxmoxVmId);
                _context.UserLabVms.Remove(userLabVm);
                await _context.SaveChangesAsync();
            }

            var bridgeInstances = userLab.BridgeInstances.ToList();
            foreach (var bridge in bridgeInstances)
            {
                await api.DestroyBridge(bridge.HypervisorInterfaceId);
                _context.BridgeInstances.Remove(bridge);
                await _context.SaveChangesAsync();
            }

            userLab.Status = EUserLabStatus.Completed;
            await _context.SaveChangesAsync();
        }
    }
}