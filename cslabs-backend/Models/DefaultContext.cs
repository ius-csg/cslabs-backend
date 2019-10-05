using System;
using CSLabsBackend.Util;
using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options)
            : base(options)
        { }
        
        
        public DbSet<Module> Modules { get; set; }
        
        public DbSet<Badge> Badges { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Lab> Labs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
       {
           base.OnModelCreating(builder);
           Module.OnModelCreating(builder);
           Badge.OnModelCreating(builder);
           User.OnModelCreating(builder);
           Lab.OnModelCreating(builder);
           builder.SnakeCaseDatabase();
       }
       
       public override int SaveChanges(bool acceptAllChangesOnSuccess)
       {
           ContextUtil.UpdateTimeStamps(ChangeTracker);
           return base.SaveChanges(acceptAllChangesOnSuccess);
       }

    }
}
