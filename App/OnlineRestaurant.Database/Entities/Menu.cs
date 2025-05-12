using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class Menu : BaseEntity
    {   
        // foreign keys 
        public int FoodCategoryId { get; set; }
        public FoodCategory FoodCategory { get; set; }

        //references
        public ICollection<Item> Items { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<MenuItemConfiguration> ItemConfigurations { get; set; }
    }
}
