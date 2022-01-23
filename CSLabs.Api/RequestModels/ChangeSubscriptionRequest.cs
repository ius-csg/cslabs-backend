using System.ComponentModel.DataAnnotations;
using CSLabs.Api.Models.UserModels;

namespace CSLabs.Api.RequestModels
{
    public class ChangeSubscriptionRequest
    {
        [Required]
        public bool Subscribe { get; set; }
    }
}