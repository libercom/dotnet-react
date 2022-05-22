using Microsoft.EntityFrameworkCore;
using domain.Models;

namespace domain.Context
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
            modelBuilder.Entity<Order>()
                .HasOne(o => o.SendingCountry)
                .WithMany()
                .HasForeignKey(o => o.SendingCountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.DestinationCountry)
                .WithMany()
                .HasForeignKey(o => o.DestinationCountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
