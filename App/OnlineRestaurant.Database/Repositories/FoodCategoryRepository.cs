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
    public class FoodCategoryRepository : BaseRepository<FoodCategory>, IFoodCategoryRepository
    {
        public FoodCategoryRepository(OnlineRestaurantDbContext context) : base(context) 
        {
            
        }

        public async Task<bool> IsTypeUniqueAsync(FoodCategory category)
        {
            return !(await GetRecords().AnyAsync(c => c.Type == category.Type));
        }
    }
}
