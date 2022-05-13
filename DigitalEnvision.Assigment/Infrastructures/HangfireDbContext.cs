using Microsoft.EntityFrameworkCore;

namespace DigitalEnvision.Assigment.Infrastructures
{
    public class HangfireDbContext : DbContext
    {
        public HangfireDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbContext DbContext => DbContext;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
