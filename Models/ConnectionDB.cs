using Microsoft.EntityFrameworkCore;

namespace MVCTutorial.Models
{
    public class ConnectionDB : DbContext
    {
        public ConnectionDB(DbContextOptions<ConnectionDB> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); // Habilita Lazy Loading
        }
        public DbSet<Employee> Employee { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Sites> Sites {  get; set; } 

        public DbSet<SiteUser> SiteUser { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

    }
}


