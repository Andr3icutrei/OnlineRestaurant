using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public interface IAllergenRepository : IBaseRepository<Allergen>
    {
        Task<bool> IsTypeUniqueAsync(Allergen type);
        Task<bool> IsTypeUniqueStoredAsync(Allergen allergen);
    }
}
