using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public interface IMenuService
    {
        Task AddMenuAsync(Menu menu);
        Task<IEnumerable<Menu>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<IEnumerable<Item?>> GetItemsForMenuAsync(int id);
        Task UpdateAsync(Menu menu);
        Task<Menu?> GetMenuWithReferencesAsync(int id);
    }
}
