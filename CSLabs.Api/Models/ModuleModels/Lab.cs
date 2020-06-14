using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models.Enums;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSLabs.Api.Models.ModuleModels
{
    public class Lab : Trackable, IPrimaryKeyModel, IValidatableObject
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ELabType Type { get; set; } = ELabType.Permanent;

        [Required]
        public int ModuleId { get; set; }
        [ForeignKey(nameof(ModuleId))]
        [JsonIgnore]
        public Module Module { get; set; }
        
        public int LabDifficulty { get; set; }
        
        [InverseProperty(nameof(LabVm.Lab))]
        public List<LabVm> LabVms { get; set; } = new List<LabVm>();
        
        public int EstimatedCpusUsed { get; set; }
        public int EstimatedMemoryUsedMb { get; set; }

        [InverseProperty(nameof(BridgeTemplate.Lab))]
        public List<BridgeTemplate> BridgeTemplates { get; set; } = new List<BridgeTemplate>();
        [NotMapped]
        public bool HasTopology { get; set; }
        [NotMapped]
        public bool HasReadme { get; set; }
        [NotMapped]
        public bool HasUserLabs { get; set; }

        public async Task updateHasUserLabs(DefaultContext context)
        {
            HasUserLabs = await context.UserLabs.Where(ul => ul.Lab == this).AnyAsync();
        }
        
        [InverseProperty(nameof(UserLab.Lab))]
        [JsonIgnore]
        public List<UserLab> UserLabs { get; set; } = new List<UserLab>();
        public string GetTopologyPath()
        {
            return "Assets/Images/" + Id + ".jpg";
        }
        public string GetReadmePath()
        {
            return "Assets/Pdf/" + Id + ".pdf";
        }

        public void DetectAttachments()
        {
            if (File.Exists(GetTopologyPath()))
                HasTopology = true;
            if (File.Exists(GetReadmePath()))
                HasReadme = true;
        }

        public void LinkBridgeTemplates()
        {
            var bridgeTemplates = BridgeTemplates.ToDictionary(t => t.Uuid);
            foreach (var vm in LabVms)
            {
                foreach (var templateInterface in vm.TemplateInterfaces)
                {
                    templateInterface.BridgeTemplate = bridgeTemplates[templateInterface.BridgeTemplateUuid];
                }
            }
        }

        public async Task DeleteMissingProperties(DefaultContext context)
        {
            var labVmIds = LabVms.Select(t => t.Id).ToList();
            var vmsToRemove = await context.LabVms
                .Where(t => t.LabId == Id && !labVmIds.Contains(t.Id)).ToListAsync();
            context.LabVms.RemoveRange(vmsToRemove);
            
            var interfaceIds = LabVms.SelectMany(t => t.TemplateInterfaces).Select(i => i.Id).ToList();
            var interfacesToRemove = await context.VmInterfaceTemplates
                .Where(t => labVmIds.Contains(t.LabVmId) && !interfaceIds.Contains(t.Id)).ToListAsync();
            context.VmInterfaceTemplates.RemoveRange(interfacesToRemove);
            
            var bridgeTemplateIds = BridgeTemplates.Select(t => t.Id).ToList();
            var bridgeTemplatesToRemove = await context.BridgeTemplates
                .Where(t => t.LabId == Id && !bridgeTemplateIds.Contains(t.Id)).ToListAsync();
            context.BridgeTemplates.RemoveRange(bridgeTemplatesToRemove);
            
        }

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
            builder.Unique<Lab>(u => new {u.Name, u.ModuleId});
            builder.Entity<Lab>().Property(p => p.Type).HasConversion<string>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return validationContext.ValidateUnique<Lab>("name", Name, Id,
                "A Lab with this name already exists", query => query.Where(m => m.ModuleId == ModuleId));
        }
    }
}
