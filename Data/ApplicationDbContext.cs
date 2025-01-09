using RRS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace RRS.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions<ApplicationDbContext> as parameter
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Menu> Menu { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
