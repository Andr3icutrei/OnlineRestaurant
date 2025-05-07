using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class OrderMenu : BaseEntity
    {
        //foreign keys 
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
