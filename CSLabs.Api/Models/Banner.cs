using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models

{
    public class Banner : Trackable
    {

        public int Id { get; set; }
        [Required]
        
        public string type { get; set; }
        [Column(TypeName = "TEXT")]
        [Required]
        
        public string description { get; set; }
        [Column(TypeName = "VARCHAR(255)")]
        [Required]
        
       public int RequiredNumOfModules { get; set; }

        public static void OnAlertCreation(ModelBuilder builder)
        {
            builder.TimeStamps<Banner>();
            builder.Unique<Banner>(u => u.type);
        }
       
    }
}