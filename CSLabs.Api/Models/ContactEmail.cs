using System.ComponentModel.DataAnnotations.Schema;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models
{
    public class ContactEmail
    {
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR(100)")]
        public string Email { get; set; }
        
        public static void OnModelCreating(ModelBuilder builder)
        {
            builder.Unique<ContactEmail>(u => u.Email);
        }
    }
}