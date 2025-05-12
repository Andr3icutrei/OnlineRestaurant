using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public interface IFoodCategoryService
    {
        Task AddFoodCategoryAsync(FoodCategory foodCategory);
        Task<bool> IsTypeUniqueAsync(FoodCategory foodCategory);
        IEnumerable<FoodCategory> GetAll();
    }
}
