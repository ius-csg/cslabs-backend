using Microsoft.EntityFrameworkCore;

namespace CSLabsBackend.Models
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options)
            : base(options)
        { }
        // add tables here
       // public DbSet<SomeModel> SomeModelName { get; set; }
    }
}
