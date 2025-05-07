using OnlineRestaurant.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class FoodCategory : BaseEntity
    {
        public FoodCategoryType Type { get; set; }

        // references 
        public ICollection<Menu> Menus { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
