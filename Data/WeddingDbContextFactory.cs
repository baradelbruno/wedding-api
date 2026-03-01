using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WeddingApi.Data
{
    public class WeddingDbContextFactory : IDesignTimeDbContextFactory<WeddingDbContext>
    {
        public WeddingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeddingDbContext>();
            
            // Use PostgreSQL for migrations (temporary connection string)
            optionsBuilder.UseNpgsql("Host=localhost;Database=wedding_design;Username=postgres;Password=postgres");

            return new WeddingDbContext(optionsBuilder.Options);
        }
    }
}