using System.ComponentModel.DataAnnotations;
using CSLabs.Api.Models.UserModels;

namespace CSLabs.Api.RequestModels
{
    public class ChangeNewsletterSubscriptionRequest
    {
        [Required]
        public bool Subscribe { get; set; }
    }
}