using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class Menu : BaseEntity
    {
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; }
    
        // foreign keys 
        public int? FoodCategoryId { get; set; }
        public FoodCategory? FoodCategory { get; set; }

        //references
        public ICollection<MenuOrder> Orders { get; set; }
        public ICollection<MenuItemConfiguration> ItemConfigurations { get; set; }
    }
}
