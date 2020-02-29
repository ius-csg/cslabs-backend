using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Models.ModuleModels;

namespace CSLabs.Api.Models.HypervisorModels
{
    public class VmTemplate : IQemu
    {
        public int Id { get; set; }
        
        public bool IsCoreRouter { get; set; }
        public int HypervisorNodeId  { get; set; }
        [ForeignKey(nameof(HypervisorNodeId))]
        public HypervisorNode HypervisorNode { get; set; }
        
        public int TemplateVmId { get; set; }
        
        public int LabVmId  { get; set; }
        [ForeignKey(nameof(LabVmId))]
        public LabVm LabVm { get; set; }

       
    }
}