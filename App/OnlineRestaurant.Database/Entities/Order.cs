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
        public int UserId { get; set; }
        public User User { get; set; }

        // references
        public ICollection<OrderItem> OrdersItems { get; set; }
        public ICollection<OrderMenu> OrderMenus { get; set; }
    }
}
