using ExciseLicenceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExciseLicenceApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Expose the Challan collection (Only thing added here!)
        public DbSet<ChallanCheck> ChallanChecks { get; set; }
    }
}