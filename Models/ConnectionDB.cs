using Microsoft.EntityFrameworkCore;

namespace MVCTutorial.Models
{
    public class ConnectionDB : DbContext
    {
        private readonly IConfiguration _configuration;

        public ConnectionDB(IConfiguration configuration, DbContextOptions<ConnectionDB> options)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    var connectionString = _configuration.GetConnectionString("DefaultConnection");
            //    optionsBuilder.UseSqlServer(connectionString);
            //    optionsBuilder.UseLazyLoadingProxies();
            //}
        }
        public DbSet<Employee> Employee { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<Sites> Sites {  get; set; } 

        public DbSet<SiteUser> SiteUser { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<ImageStore> ImageStore { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }


    }
}


