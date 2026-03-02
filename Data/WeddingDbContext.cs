using Microsoft.EntityFrameworkCore;
using WeddingApi.Classes;

namespace WeddingApi.Data
{
    public class WeddingDbContext : DbContext
    {
        public WeddingDbContext(DbContextOptions<WeddingDbContext> options) : base(options)
        {
        }

        public DbSet<WeddingGuest> WeddingGuests { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<GiftPurchase> GiftPurchases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Suppress pending model changes warning
            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Gift-GiftPurchase relationship (one-to-many)
            modelBuilder.Entity<Gift>()
                .HasMany(g => g.Purchases)
                .WithOne(p => p.Gift)
                .HasForeignKey(p => p.GiftId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed some initial data for WeddingGuests
            modelBuilder.Entity<WeddingGuest>().HasData(
                new WeddingGuest
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = null,
                    PhoneNumber = null,
                    IsAttending = false
                },
                new WeddingGuest
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = null,
                    PhoneNumber = null,
                    IsAttending = false
                }
            );
        }
    }
}