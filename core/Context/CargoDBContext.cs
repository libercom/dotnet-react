using Microsoft.EntityFrameworkCore;
using core.Models;

namespace core.Context
{
    public class CargoDBContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CargoType> CargoTypes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=.;Database=CargoProject;Trusted_Connection=True;MultipleActiveResultSets=True;")
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.SendingCountry)
                .WithOne()
                .HasForeignKey<Order>(o => o.SendingCountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.DestinationCountry)
                .WithOne()
                .HasForeignKey<Order>(o => o.DestinationCountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
