using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task AddMenuAsync(Menu entity)
        {
            _menuRepository.Insert(entity);
            await _menuRepository.SaveChangesAsync();
        }

        public async Task<decimal> CalculateMenuPriceAsync(int id)
        {
            return await _menuRepository.CalculateMenuPriceStoredAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _menuRepository.SoftDeleteByIdAsync(id);
        }

        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            return await _menuRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Item?>> GetItemsForMenuAsync(int id)
        {
            return await _menuRepository.GetItemsForMenuStoredAsync(id);
        }

        public async Task<IEnumerable<Menu>?> GetMenusWithReferences()
        {
            return await _menuRepository.GetMenusWithReferences();
        }

        public async Task<Menu?> GetMenuWithReferencesAsync(int id)
        {
            return await _menuRepository.GetMenuWithReferencesAsync(id);
        }

        public async Task UpdateAsync(Menu menu)
        {
            await _menuRepository.Update(menu);
        }
    }
}
