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
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(OnlineRestaurantDbContext context) : base(context) 
        {
        }

        public async Task<IEnumerable<Item?>> GetItemsForMenuAsync(int id)
        {
            return await _databaseContext.MenuItemConfigurations
                .Where(mic => mic.MenuId == id && mic.DeletedAt == null)
                .Include(mic => mic.Item)
                .Select(mic => mic.Item)
                .ToListAsync();
        }

        public async Task<Menu?> GetMenuWithReferencesAsync(int id)
        {
            return await _databaseContext.Menus.Include(m => m.FoodCategory).
                Include(m => m.ItemConfigurations.Where(mic => mic.DeletedAt == null)).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
