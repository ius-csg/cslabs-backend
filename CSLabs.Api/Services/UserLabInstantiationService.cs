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

        public async Task Instantiate(UserLab userLab, ProxmoxManager ProxmoxManager, RootRouterSettings routerSettings)
        {
            var node = await ProxmoxManager.GetLeastLoadedHyperVisorNode(userLab.Lab);
            var api = ProxmoxManager.GetProxmoxApi(GetFirstAvailableHypervisorNodeFromTemplates(userLab.Lab.LabVms));
            List<UserLabVm> vms = new List<UserLabVm>();

            var lanInterface = new HypervisorNetworkInterface {InterfaceId = await api.CreateBridge()};

            userLab.Interfaces = new List<HypervisorNetworkInterface> {lanInterface};
            int createdRouterId = await api.CloneTemplate(node, routerSettings.TemplateId, routerSettings.Node);
            var router = new UserLabVm
            {
                Interfaces = new List<HypervisorNetworkInterface> {lanInterface},
                IsRootRouter = true,
                ProxmoxVmId = createdRouterId
            };
            vms.Add(router);
            
            foreach (var labVm in userLab.Lab.LabVms)
            {
                var template = labVm.GetTemplateWithNode(api.HypervisorNode);
                int createdVmId = await api.CloneTemplate(node, template.TemplateVmId);
                vms.Add(new UserLabVm
                {
                    LabVm = labVm,
                    ProxmoxVmId = createdVmId,
                    VmTemplate = template,
                    Interfaces = new List<HypervisorNetworkInterface> {lanInterface}
                });
            }

            foreach (var vm in vms)
            {
                await api.AddBridgeToVm(vm.ProxmoxVmId, lanInterface.InterfaceId, vm.IsRootRouter ? 1 : 0, node.Name);
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
    }
}