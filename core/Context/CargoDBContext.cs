using Microsoft.EntityFrameworkCore;
using domain.Models;
using core.Context.Configurations;
using core.Extensions;

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

        public CargoDBContext(DbContextOptions<CargoDBContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new OrderConfig().Configure(modelBuilder.Entity<Order>());
            new UserConfig().Configure(modelBuilder.Entity<User>());

            modelBuilder.Seed();
        }
    }
}
