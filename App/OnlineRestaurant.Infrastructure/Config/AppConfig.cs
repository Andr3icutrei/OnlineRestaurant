using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Infrastructure.Config
{
    public static class AppConfig
    {
        public static bool ConsoleLogQueries = true;
        public static ConnectionStrings? Strings { get; set; }

        public static void Init(IConfiguration configuration)
        {
            Configure(configuration);
        }

        private static void Configure(IConfiguration configuration)
        {
            Strings = configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
        }
    }
}
