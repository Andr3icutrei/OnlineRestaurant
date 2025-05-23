﻿using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public interface IItemRepository : IBaseRepository<Item>
    {
        Task<Item?> GetItemWithReferencesAsync(int id);
        Task<IEnumerable<Item>> GetAllItemsWithReferencesAsync();
        IEnumerable<Item> GetAllItemsWithReferences();
    }
}
