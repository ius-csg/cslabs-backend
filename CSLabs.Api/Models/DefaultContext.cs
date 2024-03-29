﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CSLabs.Api.Models
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options)
            : base(options)
        { }

        public DbSet<Badge> Badges { get; set; }
        
        public DbSet<User> Users { get; set; }
        
        public DbSet<Module> Modules { get; set; }
        
        public DbSet<Tag> Tags { get; set; }
        
        public DbSet<Lab> Labs { get; set; }
        
        public DbSet<LabVm> LabVms { get; set; }
        public DbSet<UserModule> UserModules { get; set; }
        
        public DbSet<UserLab> UserLabs { get; set; }
        
        public DbSet<UserLabVm> UserLabVms { get; set; }
        
        public DbSet<Hypervisor> Hypervisors { get; set; }
        public DbSet<HypervisorNode> HypervisorNodes { get; set; }
        
        public DbSet<ContactEmail> ContactEmails { get; set; }
        
        public DbSet<BridgeTemplate> BridgeTemplates { get; set; }
        public DbSet<BridgeInstance> BridgeInstances { get; set; }
        
        public DbSet<VmInterfaceTemplate> VmInterfaceTemplates { get; set; }
        public DbSet<VmInterfaceInstance> VmInterfaceInstances { get; set; }
        
        public DbSet<HypervisorVmTemplate> HypervisorVmTemplates { get; set; }

        public DbSet<SystemMessage> SystemMessages { get; set; }
        public DbSet<VmTemplate> VmTemplates { get; set; }
        
        public DbSet<Maintenance> Maintenances { get; set; }
        
        public DbSet<SystemStatus> SystemStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
       {
           base.OnModelCreating(builder);
           
           Badge.OnModelCreating(builder);
           User.OnModelCreating(builder);
           
           // module section
           Module.OnModelCreating(builder);
           Tag.OnModelCreating(builder);
           ModuleTag.OnModelCreating(builder);
           Lab.OnModelCreating(builder);
           LabVm.OnModelCreating(builder);
           UserModule.OnModelCreating(builder);
           // user module section
           UserModule.OnModelCreating(builder);
           UserLab.OnModelCreating(builder);
           UserLabVm.OnModelCreating(builder);
           // hypervisor
           Hypervisor.OnModelCreating(builder);
           HypervisorNode.OnModelCreating(builder);
           // configure many to many relationship
           UserUserModule.OnModelCreating(builder);
           ContactEmail.OnModelCreating(builder);
           BridgeInstance.OnModelCreating(builder);
           VmInterfaceTemplate.OnModelCreating(builder);
           VmInterfaceInstance.OnModelCreating(builder);
           BridgeTemplate.OnModelCreating(builder);
           VmTemplate.OnModelCreating(builder);

           SystemMessage.OnModelCreating(builder);
       }
       
       public override int SaveChanges(bool acceptAllChangesOnSuccess)
       {
           ContextUtil.UpdateTimeStamps(ChangeTracker);
           return base.SaveChanges(acceptAllChangesOnSuccess);
       }

       public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
       {
           ContextUtil.UpdateTimeStamps(ChangeTracker);
           return base.SaveChangesAsync(cancellationToken);
       }
       
       /**
         * Removed all entities from the change tracker so that any additional
         * data pulled from the database is not "auto fix-up" by values in the change tracker.
         * @see https://docs.microsoft.com/en-us/ef/core/querying/related-data and search "fix-up"
         */
       public void DetachAllEntities()
       {
           var changedEntriesCopy = this.ChangeTracker.Entries().ToList();
           // The two lines below are needed to prevent an InvalidOperationException from being thrown.
           // @see https://github.com/dotnet/efcore/issues/18406
           ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
           ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
           foreach (var entry in changedEntriesCopy)
           {
               entry.State = EntityState.Detached;
           }
       }
    }
}
