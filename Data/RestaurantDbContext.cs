using AppRestaurant.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;
using System.IO;

namespace AppRestaurant.Data
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "restaurant.db");
                optionsBuilder.UseSqlite($"Filename={dbPath}");
            }
        }
    }
}
