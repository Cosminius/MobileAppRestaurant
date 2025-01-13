using AppRestaurant.Models;
using System;
using System.Linq;

namespace AppRestaurant.Data
{
    public static class DatabaseService
    {
        public static void InitializeDatabase()
        {
            using (var db = new RestaurantDbContext())
            {
                
                db.Database.EnsureCreated();

                
                if (!db.Categories.Any())
                {
                    SeedData(db);
                }
            }
        }

        private static void SeedData(RestaurantDbContext db)
        {
            
            var categories = new[]
            {
                new Category { Name = "Aperitive" },
                new Category { Name = "Fel Principal" },
                new Category { Name = "Deserturi" },
                new Category { Name = "Bauturi" }
            };
            db.Categories.AddRange(categories);
            db.SaveChanges();


            var products = new[]
            {
                new Product
                {
                    Name = "Rulouri de Primavara",
                    Description = "Rulouri crocante",
                    Price = 5.99m,
                    CategoryId = categories[0].CategoryId,
                    ImageUrl = "spring.jpg"
                },
                new Product
                {
                    Name = "Pui la Gratar",
                    Description = "Pui picant la gratar",
                    Price = 12.99m,
                    CategoryId = categories[1].CategoryId,
                    ImageUrl = "pui.jpg"
                },
                 new Product
                {
                    Name = "Saramale",
                    Description = "Sarmale ca la mama acasa",
                    Price = 8.99m,
                    CategoryId = categories[1].CategoryId,
                    ImageUrl = "sarmale.jpg"
                }
            };
            db.Products.AddRange(products);
            db.SaveChanges();

            
            var notifications = new[]
            {
                new Notification
                {
                    Title = "Bine ai venit!",
                    Message = "Iti multumim ca folosesti aplicatia noastra!",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                },
                 new Notification
                {
                    Title = "Va multumim pentru comanda plasata",
                    Message = "Pofta buna",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                }
            };
            db.Notifications.AddRange(notifications);
            db.SaveChanges();
        }
    }
}
