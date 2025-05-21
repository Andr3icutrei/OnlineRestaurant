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
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(OnlineRestaurantDbContext databaseContext) : base(databaseContext)
        {
        }

        public IEnumerable<int> GetAllItemQuantities(int orderId)
        {
            return _databaseContext.ItemOrder.Where(io => io.OrdersId == orderId).
                                  OrderBy(io => io.ItemsId).
                                  Select(io => io.NoItems).
                                  ToList();
        }

        public IEnumerable<Item> GetAllItems(int orderId)
        {
            return _databaseContext.ItemOrder.Where(io => io.OrdersId == orderId).
                                              Include(io => io.Item).
                                              Select(io => io.Item).
                                              OrderBy(i => i.Id).
                                              ToList();
        }

        public IEnumerable<int> GetAllMenuQuantites(int orderId)
        {
            return _databaseContext.MenuOrder.Where(mo => mo.OrdersId == orderId).
                      OrderBy(mo => mo.MenusId).
                      Select(mo => mo.NoMenus).
                      ToList();
        }

        public IEnumerable<Menu> GetAllMenus(int orderId)
        {
            return _databaseContext.MenuOrder.Where(mo => mo.OrdersId == orderId).
                                              Include(mo => mo.Menu).
                                              Select(mo => mo.Menu).
                                              OrderBy(m => m.Id).
                                              ToList();
        }

        public IEnumerable<Order> GetAllWithReferences()
        {
            return _databaseContext.Orders.Include(o => o.Menus).
                                           Include(o => o.Items).
                                           OrderBy(o => o.Id).
                                           ToList();
        }

        public IEnumerable<Order> GetAllWithReferencesAsc()
        {
            return _databaseContext.Orders.Include(o => o.Menus)
                                          .Include(o => o.Items)
                                          .OrderBy(o => o.CreatedAt)
                                          .ToList();
        }

        public IEnumerable<Order> GetAllWithReferencesDesc()
        {
            return _databaseContext.Orders.Include(o => o.Menus)
                              .Include(o => o.Items)
                              .OrderByDescending(o => o.CreatedAt)
                              .ToList();
        }
    }
}
