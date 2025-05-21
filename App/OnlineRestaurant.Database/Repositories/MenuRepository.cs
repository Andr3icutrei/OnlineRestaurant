using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineRestaurant.Database.Context;
using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data;
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
                .OrderBy(mic => mic.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item?>> GetItemsForMenuStoredAsync(int id)
        {
            var parameter = new SqlParameter("@MenuId", id);

            return await _databaseContext.Items
                .FromSqlRaw("EXEC GetItemsForMenu @MenuId", parameter)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Menu?> GetMenuWithReferencesAsync(int id)
        {
            return await _databaseContext.Menus.
                Include(m => m.FoodCategory).
                Include(m => m.ItemConfigurations.Where(mic => mic.DeletedAt == null)).
                FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Menu>> GetMenusWithReferences()
        {
            return await _databaseContext.Menus
                .Include(m => m.FoodCategory)
                .Include(m => m.ItemConfigurations.Where(ic => ic.DeletedAt == null))
                .OrderBy(m => m.Id)
                .ToListAsync();
        }

        public async Task<decimal> CalculateMenuPriceAsync(int id)
        {
            var menu = await _databaseContext.Menus
                .Where(m => m.Id == id)
                .Include(m => m.ItemConfigurations.Where(mic => mic.DeletedAt == null))
                .ThenInclude(mic => mic.Item)
                .FirstOrDefaultAsync();

            if(menu == null)
                return 0;

            return menu.ItemConfigurations.Sum(mic => mic.Item.Price);
        }

        public async Task<decimal> CalculateMenuPriceStoredAsync(int id)
        {
            var totalPriceParameter = new SqlParameter
            {
                ParameterName = "@TotalPrice",
                SqlDbType = SqlDbType.Decimal,
                Precision = 10,
                Scale = 2,
                Direction = ParameterDirection.Output
            };

            var menuIdParameter = new SqlParameter
            {
                ParameterName = "@MenuId",
                SqlDbType = SqlDbType.Int,
                Value = id
            };

            await _databaseContext.Database.ExecuteSqlRawAsync(
                "EXEC CalculateMenuPrice @MenuId, @TotalPrice OUTPUT",
                menuIdParameter, totalPriceParameter);

            return totalPriceParameter.Value != DBNull.Value
                ? Convert.ToDecimal(totalPriceParameter.Value)
                : 0m;
        }
    } 
}
