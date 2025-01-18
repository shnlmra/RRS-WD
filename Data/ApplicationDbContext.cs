using Microsoft.EntityFrameworkCore;
using RRS.Models;

namespace RRS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Table> Tables { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Menu>()
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Table>()
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Table>()
                .Property(m => m.Status)
                .HasDefaultValue("available");
        }
    }
}
