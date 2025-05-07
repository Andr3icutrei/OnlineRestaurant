using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Enums
{
    public enum AllergenType
    {
        InvalidAllergenType = 0,
        Eggs = 1,
        Gluten = 2,
        Peanuts = 4,
        Milk = 8,
        Fish = 16,
        Soy = 32
    }
}
