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
		public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }
    }
}
