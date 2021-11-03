using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;

namespace CSLabs.Tests
{
    public abstract class DatabaseTest
    {
        protected DefaultContext _context;
        protected DbContextOptions<DefaultContext> dbOptions;
        private IDbContextTransaction _transaction;
        
        [SetUp]
        public virtual async Task Setup()
        {
            
            var optionsBuilder = new DbContextOptionsBuilder<DefaultContext>();
            ServiceProvider.ConfigureMysql(optionsBuilder, "Server=127.0.0.1;Database=cslabs_backend_testing;User=root;Password=;");
            dbOptions = optionsBuilder.Options;
            _context = new DefaultContext(dbOptions);
            await _context.Database.MigrateAsync();
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _transaction.RollbackAsync();
            _context.DetachAllEntities();
        }
    }
}