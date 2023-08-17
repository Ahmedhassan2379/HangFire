using Microsoft.EntityFrameworkCore;

namespace HangFire.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
