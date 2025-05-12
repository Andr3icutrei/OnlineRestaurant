using OnlineRestaurant.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class Allergen : BaseEntity
    {
        public string Type { get; set; }

        // references
        public ICollection<Item> Items { get; set; }
    }
}
