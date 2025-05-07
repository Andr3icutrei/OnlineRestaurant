using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Context
{
    public class OnlineRestaurantDbContextFactory : IDesignTimeDbContextFactory<OnlineRestaurantDbContext>
    {
        public OnlineRestaurantDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<OnlineRestaurantDbContext>();
            var connectionString = configuration.GetConnectionString("OnlineRestaurantDatabase");

            builder.UseSqlServer(connectionString);

            return new OnlineRestaurantDbContext(builder.Options);
        }
    }
}
