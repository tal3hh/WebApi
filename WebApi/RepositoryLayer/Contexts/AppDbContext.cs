using DomainLayer.Configurations;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User>? Members { get; set; }
        public DbSet<Country>? Countries { get; set; }
        public DbSet<City>? Cities { get; set; }
    }
}
