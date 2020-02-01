using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Models.UserModels
{
    /**
     * This model is a many to many mapping of users to user_labs.
     */
    public class UserUserModule
    {
        public int UserId  { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        
        public int UserModuleId  { get; set; }
        [ForeignKey(nameof(UserModuleId))]
        public UserModule UserModule { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<UserUserModule>()
                .HasKey(t => new { t.UserId, t.UserModuleId });

            modelBuilder.Entity<UserUserModule>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserUserModules)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserUserModule>()
                .HasOne(pt => pt.UserModule)
                .WithMany(t => t.UserUserModules)
                .HasForeignKey(pt => pt.UserModuleId);
        }
    }
}