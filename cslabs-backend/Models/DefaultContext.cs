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
       
       
       protected override void OnModelCreating(ModelBuilder builder)
       {
           base.OnModelCreating(builder);
           
           Module.OnModelCreating(builder);
           Badge.OnModelCreating(builder);
       
           foreach(var entity in builder.Model.GetEntityTypes())
           {
               // Replace table names
               entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

               // Replace column names            
               foreach(var property in entity.GetProperties())
               {
                   property.Relational().ColumnName = property.Relational().ColumnName.ToSnakeCase();
               }

               foreach(var key in entity.GetKeys())
               {
                   key.Relational().Name = key.Relational().Name.ToSnakeCase();
               }

               foreach(var key in entity.GetForeignKeys())
               {
                   key.Relational().Name = key.Relational().Name.ToSnakeCase();
               }

               foreach(var index in entity.GetIndexes())
               {
                   index.Relational().Name = index.Relational().Name.ToSnakeCase();
               }
           }
       }
       
       public override int SaveChanges(bool acceptAllChangesOnSuccess)
       {
           OnBeforeSaving();
           return base.SaveChanges(acceptAllChangesOnSuccess);
       }
       private void OnBeforeSaving()
       {
           // Got Code from
           // https://www.meziantou.net/entity-framework-core-generate-tracking-columns.htm
           var entries = ChangeTracker.Entries();
           foreach (var entry in entries)
           {
               if (entry.Entity is ITrackable trackable)
               {
                   DateTime now = DateTime.UtcNow;
                   switch (entry.State)
                   {
                       case EntityState.Modified:
                           trackable.UpdatedAt = now;
                           break;

                       case EntityState.Added:
                           trackable.CreatedAt = now;
                           trackable.UpdatedAt = now;
                           break;
                   }
               }
           }
       }
       
    }
}
