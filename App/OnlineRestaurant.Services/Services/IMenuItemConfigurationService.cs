using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public interface IMenuItemConfigurationService
    {
        Task AddMenuItemConfigurationAsync(MenuItemConfiguration menuItemConfiguration);
        Task UpdateAsync(MenuItemConfiguration menuItemConfiguration);
    }
}
