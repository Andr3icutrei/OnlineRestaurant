using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Context
{
    public class OnlineRestaurantDbContext : DbContext
    {
        public OnlineRestaurantDbContext(DbContextOptions<OnlineRestaurantDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
                        {
                            relationship.DeleteBehavior = DeleteBehavior.Restrict;
                        }
        }

        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemAllergen> ItemsAllergens { get; set; }
        public DbSet<ItemPicture> ItemPictures { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenusItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set; }
        public DbSet<OrderMenu> OrdersMenus { get; set; }
        public DbSet<User> Users { get; set; }
     }
}
