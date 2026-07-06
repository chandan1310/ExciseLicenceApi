using System.Collections.Generic;
using ExciseLicenceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExciseLicenceApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        // The constructor passes connection settings down to EF Core
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // This property represents your database table
        public DbSet<BarFirstRegistration> BarFirstRegistrations { get; set; }
    }
}