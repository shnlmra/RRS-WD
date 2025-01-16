using Microsoft.EntityFrameworkCore;
using RRS.Models.Entities;

namespace RRS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Menu> Menus { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                   .Property(c => c.IsDeleted)
                   .HasDefaultValue(false);

            modelBuilder.Entity<Menu>()
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);

        }
    }
}
