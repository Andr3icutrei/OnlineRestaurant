using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repository;

        public ItemService(IItemRepository repository)
        {
            _repository = repository;
        }

        public async Task AddItemAsync(Item item)
        {
            _repository.Insert(item);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteByIdAsync(id);
        }

        public IEnumerable<Item> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _repository.GetAllAsync();  
        }

        public async Task<IEnumerable<Item>> GetAllItemsWithReferencesAsync()
        {
            return await _repository.GetAllItemsWithReferencesAsync();
        }

        public async Task<Item?> GetItemWithReferencesAsync(int id)
        {
            return await _repository.GetItemWithReferencesAsync(id);
        }

        public async Task UpdateAsync(Item item)
        {
            await _repository.Update(item);
        }
    }
}
