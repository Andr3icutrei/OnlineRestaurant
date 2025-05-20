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
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(OnlineRestaurantDbContext context) : base(context) 
        {
            
        }

        public async Task<IEnumerable<Item>> GetAllItemsWithReferencesAsync()
        {
            return await _databaseContext.Items.Where(i => i.DeletedAt == null).
                                                Include(i => i.Pictures).
                                                Include(i => i.Allergens).
                                                Include(i => i.FoodCategory).
                                                ToListAsync();
        }

        public async Task<Item?> GetItemWithReferencesAsync(int id)
        {
            return await _databaseContext.Items.Include(i => i.Pictures).Include(i => i.Allergens).Include(i => i.FoodCategory).
                FirstOrDefaultAsync(i => i.Id == id && i.DeletedAt == null);
        }


    }
}
