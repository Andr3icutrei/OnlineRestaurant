using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public class MenuItemConfigurationService : IMenuItemConfigurationService
    {
        private readonly IMenuItemConfigurationRepository _menuItemConfigurationRepository;
        public MenuItemConfigurationService(IMenuItemConfigurationRepository menuItemConfigurationRepository)
        {
            _menuItemConfigurationRepository = menuItemConfigurationRepository;
        }

        public async Task AddMenuItemConfigurationAsync(MenuItemConfiguration menuItemConfiguration)
        {
            _menuItemConfigurationRepository.Insert(menuItemConfiguration);
            await _menuItemConfigurationRepository.SaveChangesAsync();
        }
    }
}
