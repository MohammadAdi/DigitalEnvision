using DigitalEnvision.Assigment.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalEnvision.Assigment.Infrastructures
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        DbSet<User> Users { get; set; }
    }
}
