using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CSLabs.Api.Models.ModuleModels
{
    public class LabVm: Trackable
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }
        [Required]
        public int LabId { get; set; }
        [ForeignKey(nameof(LabId))]
        [JsonIgnore]
        public Lab Lab { get; set; }
        
        public bool IsCoreRouter { get; set; }

        public int VmTemplateId  { get; set; }
        [ForeignKey(nameof(VmTemplateId))]
        public VmTemplate VmTemplate { get; set; }
        
        [InverseProperty(nameof(VmInterfaceTemplate.LabVm))]
        public List<VmInterfaceTemplate> TemplateInterfaces { get; set; } = new List<VmInterfaceTemplate>();

        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.TimeStamps<LabVm>();
            builder.Unique<LabVm>(u => new {u.Name, u.LabId});
        }

        public HypervisorVmTemplate GetTemplateWithNode(HypervisorNode node)
        {
            return VmTemplate.HypervisorVmTemplates.First(t => t.HypervisorNode == node);
        }
    }
}