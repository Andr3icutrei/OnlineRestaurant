using OnlineRestaurant.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class Order : BaseEntity
    {
        public OrderState State { get; set; }
        
        // foreign keys 
        public int? UserId { get; set; }
        public User? User { get; set; }

        // references
        public ICollection<Item> Items { get; set; }
        public ICollection<Menu> Menus { get; set; }
    }
}
