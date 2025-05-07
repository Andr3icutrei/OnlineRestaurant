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
        public AllergenType Type { get; set; }

        // references
        public ICollection<ItemAllergen> ItemsAllergens { get; set; }
    }
}
