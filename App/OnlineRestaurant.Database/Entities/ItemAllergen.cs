using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class ItemAllergen : BaseEntity
    {
        public int AllergenId { get; set; }
        public Allergen Allergen { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
