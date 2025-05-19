using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<FoodCategory>()
                .HasIndex(fc => fc.Type)
                .IsUnique();

            modelBuilder.Entity<Allergen>()
                .HasIndex(a => a.Type)
                .IsUnique();
        }

        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemPicture> ItemPictures { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MenuItemConfiguration> MenuItemConfigurations { get; set; }
     }
}
