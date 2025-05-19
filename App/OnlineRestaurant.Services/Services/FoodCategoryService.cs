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

        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteByIdAsync(id);
        }

        public IEnumerable<FoodCategory> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<IEnumerable<FoodCategory>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<bool> IsTypeUniqueAsync(FoodCategory category)
        {
            return await _repository.IsTypeUniqueAsync(category);
        }

        public async Task Update(FoodCategory foodCategory)
        {
            await _repository.Update(foodCategory);
        }
    }
}
