using System.ComponentModel.DataAnnotations.Schema;

namespace CSLabs.Api.Models.HypervisorModels
{
    public class HypervisorVmTemplate
    {
        public int Id { get; set; }
        public int HypervisorNodeId  { get; set; }
        [ForeignKey(nameof(HypervisorNodeId))]
        public HypervisorNode HypervisorNode { get; set; }
        
        public int TemplateVmId { get; set; }
        
        public int VmTemplateId  { get; set; }
        [ForeignKey(nameof(VmTemplateId))]
        public VmTemplate VmTemplate { get; set; }
    }
}