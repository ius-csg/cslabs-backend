using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CSLabsBackend.Models.UserModels;
using CSLabsBackend.Proxmox;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;
using static CSLabsBackend.Models.Enums.LabTypes;

namespace CSLabsBackend.Models.ModuleModels
{
    public class Lab : Trackable
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(45)")]
        public string LabType { get; set; } = Permanent;

        [Required]
        public int ModuleId { get; set; }

        // The user who created the Lab
        [Required]
        public int UserId { get; set; }
        public int LabDifficulty { get; set; }
        
        public List<LabVm> LabVms { get; set; }
        
        public int EstimatedCpusUsed { get; set; }
        public int EstimatedMemoryUsedMb { get; set; }

        /**
         * Requires LabVms.VmTemplates.HypervisorNode.Hypervisor is all included
         */
        public Hypervisor GetFirstAvailableHypervisor()
        {
            return LabVms.SelectMany(vm => vm.VmTemplates).Select(t => t.HypervisorNode.Hypervisor)
                .First();
        }
        
        public HypervisorNode GetFirstAvailableHypervisorNodeFromTemplates()
        {
            return LabVms.SelectMany(vm => vm.VmTemplates).Select(t => t.HypervisorNode).First();
        }

        public async Task<UserLab> Instantiate(ProxmoxManager ProxmoxManager, User user)
        {
            var node = await ProxmoxManager.GetLeastLoadedHyperVisorNode(this);
            var api = ProxmoxManager.GetProxmoxApi(GetFirstAvailableHypervisorNodeFromTemplates());
            List<UserLabVm> vms = new List<UserLabVm>();
            foreach (var labVm in LabVms)
            {
                var template = labVm.GetTemplateWithNode(api.HypervisorNode);
                int createdVmId = await api.CloneTemplate(node, template.TemplateVmId);
                vms.Add(new UserLabVm()
                {
                    LabVm = labVm,
                    User = user,
                    ProxmoxVmId = createdVmId,
                    VmTemplate = template
                });
            }

            return new UserLab
            {
                Lab = this,
                Status = "ON",
                User = user,
                HypervisorNode = node,
                UserLabVms = vms
            };
        }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<Lab>();
            builder.Unique<Lab>(u => u.Name);
        }
    }
}
