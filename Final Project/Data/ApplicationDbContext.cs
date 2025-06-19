using Final_Project.Models;
using Final_Project.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        {
            
        }

        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<UserLR> Users { get; set; }

        public DbSet<ItemHistory> ItemHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Fix precision warning for Inventory.Price
            modelBuilder.Entity<Inventory>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }

        
    }
}
