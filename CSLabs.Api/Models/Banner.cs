using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models

{
    public class Banner : Trackable
    {

        public int Id { get; set; }
        [Column(TypeName = "TEXT")]//need to decide on a enum
        [Required] 
        public string Type { get; set; }
       
        [Column(TypeName = "VARCHAR(255)")]
        [Required]
        public string Description { get; set; }
        
        [Column(TypeName = "datetime")]
        [Required]
        public string timestamp { get; set; }
       
        public static void OnAlertCreation(ModelBuilder builder)
        {
            builder.TimeStamps<Banner>();
            builder.Unique<Banner>(u => u.Type);
        }
       
    }
}