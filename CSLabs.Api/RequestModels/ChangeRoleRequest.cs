using System.ComponentModel.DataAnnotations;
using CSLabs.Api.Models.UserModels;

namespace CSLabs.Api.RequestModels
{
    public class ChangeRoleRequest
    {
        [Required]
        public EUserRole NewRole { get; set; }
        
        [Required]
        public int UserId { get; set; }
    }
}