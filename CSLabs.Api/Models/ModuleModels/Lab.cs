using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Proxmox;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using static CSLabs.Api.Models.Enums.LabTypes;

namespace CSLabs.Api.Models.ModuleModels
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

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<Lab>();
            builder.Unique<Lab>(u => u.Name);
        }
    }
}
