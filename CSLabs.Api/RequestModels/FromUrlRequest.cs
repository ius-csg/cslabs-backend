using System.ComponentModel.DataAnnotations;

namespace CSLabs.Api.RequestModels
{
    public class FromUrlRequest
    {
        [Url]
        public string Url { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}