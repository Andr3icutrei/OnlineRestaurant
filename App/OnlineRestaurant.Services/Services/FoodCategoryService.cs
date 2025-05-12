using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public class FoodCategoryService : IFoodCategoryService
    {
        private readonly IFoodCategoryRepository _repository;
        public FoodCategoryService(IFoodCategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task AddFoodCategoryAsync(FoodCategory foodCategory)
        {
            _repository.Insert(foodCategory);
            await _repository.SaveChangesAsync();
        }

        public IEnumerable<FoodCategory> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<bool> IsTypeUniqueAsync(FoodCategory category)
        {
            return await _repository.IsTypeUniqueAsync(category);
        }

    }
}
