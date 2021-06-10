using Core.DbModels;
using Microsoft.EntityFrameworkCore;
using MyCourseWork.Models.CartModels;

namespace MyCourseWork.Models
{
    public class HelperContext : DbContext
    {  
        public DbSet<User> Users { get; set; }

        public DbSet<Place> Places { get; set; }

        public DbSet<Role> Roles { get; set; }
        
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<OrderDetail> OrdersDetails { get; set; }

        public DbSet<CartItem> CartItem { get; set; }
        
        public HelperContext(DbContextOptions<HelperContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(s => s.Place)
                .WithMany(g => g.Places)
                .HasForeignKey(s => s.PlaceId)
                .OnDelete (DeleteBehavior.Cascade);
        }
    }
}
