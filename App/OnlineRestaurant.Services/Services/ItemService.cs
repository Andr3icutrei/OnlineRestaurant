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

        public IEnumerable<Item> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
