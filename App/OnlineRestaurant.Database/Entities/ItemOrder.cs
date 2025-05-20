using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class ItemOrder
    {
        public int ItemsId { get; set; }
        public Item Item { get; set; }

        public int OrdersId { get; set; }
        public Order Order { get; set; }

        public int NoItems { get; set; }
    }
}
