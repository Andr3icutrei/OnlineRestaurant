using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class MenuItemConfiguration : BaseEntity
    {
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        // Override for menu-specific portion
        public float? MenuPortionQuantity { get; set; }
    }
}
