using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class MenuOrder
    {
        public int MenusId { get; set; }
        public Menu Menu { get; set; }

        public int OrdersId { get; set; }
        public Order Order { get; set; }

        public int NoMenus { get; set; }
    }
}
