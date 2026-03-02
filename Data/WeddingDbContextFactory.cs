using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WeddingApi.Data
{
    public class WeddingDbContextFactory : IDesignTimeDbContextFactory<WeddingDbContext>
    {
        public WeddingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeddingDbContext>();
            
            // Update with your actual PostgreSQL password
            optionsBuilder.UseNpgsql("Host=localhost;Database=wedding_design;Username=postgres;Password=password");

            return new WeddingDbContext(optionsBuilder.Options);
        }
    }
}