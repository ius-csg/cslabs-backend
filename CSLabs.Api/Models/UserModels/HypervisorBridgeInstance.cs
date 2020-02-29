using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models.UserModels
{
    public class HypervisorBridgeInstance
    {
        public int Id { get; set; }
        [Required]
        // 0-4 is reserved, 5 and higher is used for the application.
        public int InterfaceId {get;set;}
                
        public int UserLabVmId  { get; set; }
        [ForeignKey(nameof(UserLabVmId))]
        public UserLabVm UserLabVm { get; set; }
        
        
        public int UserLabId  { get; set; }
        [ForeignKey(nameof(UserLabId))]
        public UserLab UserLab { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Unique<HypervisorBridgeInstance>(i => i.InterfaceId);
        }
    }
}