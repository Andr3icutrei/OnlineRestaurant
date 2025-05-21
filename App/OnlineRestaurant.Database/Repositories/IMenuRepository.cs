using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        Task<IEnumerable<Item?>> GetItemsForMenuAsync(int id);
        Task<Menu?> GetMenuWithReferencesAsync(int id);
        Task<IEnumerable<Menu>> GetMenusWithReferences();
        Task<decimal> CalculateMenuPriceAsync(int id);
        Task<IEnumerable<Item?>> GetItemsForMenuStoredAsync(int id);
        Task<decimal> CalculateMenuPriceStoredAsync(int id);
    }
}
