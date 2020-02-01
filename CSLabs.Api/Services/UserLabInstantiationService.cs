using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
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

        public async Task Instantiate(UserLab userLab, ProxmoxManager ProxmoxManager, User user)
        {
            var node = await ProxmoxManager.GetLeastLoadedHyperVisorNode(userLab.Lab);
            var api = ProxmoxManager.GetProxmoxApi(GetFirstAvailableHypervisorNodeFromTemplates(userLab.Lab.LabVms));
            List<UserLabVm> vms = new List<UserLabVm>();
            foreach (var labVm in userLab.Lab.LabVms)
            {
                var template = labVm.GetTemplateWithNode(api.HypervisorNode);
                int createdVmId = await api.CloneTemplate(node, template.TemplateVmId);
                vms.Add(new UserLabVm
                {
                    LabVm = labVm,
                    ProxmoxVmId = createdVmId,
                    VmTemplate = template
                });
            }

            userLab.HypervisorNode = node;
            userLab.UserLabVms = vms;
            userLab.EndDateTime = DateTime.UtcNow.AddDays(30);
            userLab.Status = EUserLabStatus.Started;
        }
    }
}