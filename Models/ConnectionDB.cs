﻿using Microsoft.EntityFrameworkCore;

namespace MVCTutorial.Models
{
    public class ConnectionDB : DbContext
    {
        public ConnectionDB() : base() { }
        public ConnectionDB(DbContextOptions<ConnectionDB> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=Quesia_NOTE;Database=MVCTutorial;Integrated Security=True;TrustServerCertificate=True;");
            optionsBuilder.UseLazyLoadingProxies();
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


