using Menu_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Menu_Management.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions<ApplicationDbContext> as parameter
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Add_Menu_Modal> Add_Menu_Modal { get; set; }

    }
}
