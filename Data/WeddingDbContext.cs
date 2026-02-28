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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed some initial data
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