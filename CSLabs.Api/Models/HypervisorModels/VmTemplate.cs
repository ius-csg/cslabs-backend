using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Models.ModuleModels;

namespace CSLabs.Api.Models.HypervisorModels
{
    public class VmTemplate : IQemu
    {
        public int Id { get; set; }

        public bool IsCoreRouter { get; set; }
        
        [InverseProperty(nameof(HypervisorVmTemplate.VmTemplate))]
        public List<HypervisorVmTemplate> HypervisorVmTemplates { get; set; } = new List<HypervisorVmTemplate>();
        
    }
}