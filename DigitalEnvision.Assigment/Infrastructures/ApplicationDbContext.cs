using DigitalEnvision.Assigment.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DigitalEnvision.Assigment.Infrastructures
{

    public interface IApplicationDbContext
    {
        DbSet<Models.User> Users { get; set; }
        DbSet<Models.Location> Locations { get; set; }
        DbSet<Models.Jobs.AlertLog> AlertLogs { get; set; }

        Task<int> SaveChanges();
    }
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Models.Jobs.AlertLog> AlertLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, LocationName = "New York", TimeZone = -4 },
                new Location { Id = 2, LocationName = "Melbourne", TimeZone = 10 }
            );
        }
        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}
