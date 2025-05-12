using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public interface IFoodCategoryRepository : IBaseRepository<FoodCategory>
    {
        Task<bool> IsTypeUniqueAsync(FoodCategory category);
    }
}
