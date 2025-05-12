using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public interface IAllergenService
    {
        Task AddAllergenAsync(Allergen allergen);
        Task<bool> IsTypeUniqueAsync(Allergen allergen);
        IEnumerable<Allergen> GetAll();
    }
}
