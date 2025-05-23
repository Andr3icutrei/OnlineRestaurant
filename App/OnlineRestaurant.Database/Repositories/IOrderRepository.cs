﻿using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        IEnumerable<Order> GetAllWithReferences();
        IEnumerable<Order> GetAllWithReferencesAsc();
        IEnumerable<Order> GetAllWithReferencesDesc();
        IEnumerable<Item> GetAllItems(int orderId);
        IEnumerable<Menu> GetAllMenus(int orderId);
        IEnumerable<int> GetAllItemQuantities(int orderId);
        IEnumerable<int> GetAllMenuQuantites(int orderId);
        IEnumerable<Item> GetAllItemsStored(int orderId);
    }
}
