using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class MenuItem : BaseEntity
    {
        public float PortionQuantity {  get; set; }
        public int NumberOfPortions { get; set; }

        // foreign keys
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
