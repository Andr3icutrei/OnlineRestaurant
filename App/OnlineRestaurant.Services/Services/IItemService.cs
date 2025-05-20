using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public interface IItemService 
    {
        Task AddItemAsync(Item item);
        IEnumerable<Item> GetAll();
        Task<IEnumerable<Item>> GetAllAsync();
        Task DeleteAsync(int id);
        Task UpdateAsync(Item item);
        Task<Item?> GetItemWithReferencesAsync(int id);
        Task<IEnumerable<Item>> GetAllItemsWithReferencesAsync();
    }
}
