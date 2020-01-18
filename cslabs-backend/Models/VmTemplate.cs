using System.ComponentModel.DataAnnotations.Schema;
using CSLabsBackend.Models.ModuleModels;

namespace CSLabsBackend.Models
{
    public class VmTemplate
    {
        public int Id { get; set; }
        public int HypervisorNodeId  { get; set; }
        [ForeignKey(nameof(HypervisorNodeId))]
        public HypervisorNode HypervisorNode { get; set; }
        
        public int TemplateVmId { get; set; }
        
        public int LabVmId  { get; set; }
        [ForeignKey(nameof(LabVmId))]
        public LabVm LabVm { get; set; }
    }
}