using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSLabs.Api.Models.UserModels
{
    public class UserInfo
    {
        
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string FirstName { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string MiddleName { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string LastName { get; set; }

        [Column(TypeName = "VARCHAR(45)")]
        public string Email { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public EUserRole Role { get; set; } = EUserRole.Guest;
        
    }
}