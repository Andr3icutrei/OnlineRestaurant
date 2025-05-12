using Microsoft.EntityFrameworkCore;
using OnlineRestaurant.Database.Context;
using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public class AllergenRepository : BaseRepository<Allergen>, IAllergenRepository
    {
        public AllergenRepository(OnlineRestaurantDbContext context) : base(context)
        {
            
        }

        public async Task<bool> IsTypeUniqueAsync(Allergen allergen)
        {
            return !(await GetRecords().AnyAsync(a => a.Type == allergen.Type));
        }
    }
}
