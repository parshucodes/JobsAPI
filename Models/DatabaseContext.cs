using Microsoft.EntityFrameworkCore;

namespace JobsAPI.Models
{
    public class DatabaseContext :DbContext
    {
        public DatabaseContext()
        {
            
        }
        public DatabaseContext(DbContextOptions<DatabaseContext>options):base(options)
        {
            
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }

        
    }
}
