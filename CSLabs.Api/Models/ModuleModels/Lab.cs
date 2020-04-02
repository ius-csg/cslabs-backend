using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CSLabs.Api.Models.Enums;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSLabs.Api.Models.ModuleModels
{
    public class Lab : Trackable
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ELabType Type { get; set; } = ELabType.Permanent;

        [Required]
        public int ModuleId { get; set; }
        
        public int LabDifficulty { get; set; }
        
        public List<LabVm> LabVms { get; set; }
        
        public int EstimatedCpusUsed { get; set; }
        public int EstimatedMemoryUsedMb { get; set; }

        [InverseProperty(nameof(BridgeTemplate.Lab))]
        public List<BridgeTemplate> BridgeTemplates { get; set; }

        /**
         * Requires LabVms.VmTemplates.HypervisorNode.Hypervisor is all included
         */
        public Hypervisor GetFirstAvailableHypervisor()
        {
            return LabVms.SelectMany(vm => vm.VmTemplate.HypervisorVmTemplates).Select(t => t.HypervisorNode.Hypervisor)
                .First();
        }

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<Lab>();
            builder.Unique<Lab>(u => u.Name);
            builder.Entity<Lab>().Property(p => p.Type).HasConversion<string>();
        }
    }
}
