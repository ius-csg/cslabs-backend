using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options)
            : base(options)
        { }
        // add tables here
       // public DbSet<UnitGroupType> SomeTableName { get; set; }
    }
}
