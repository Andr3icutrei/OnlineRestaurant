using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task AddOrder(Order order)
        {
            _orderRepository.Insert(order);
            await _orderRepository.SaveChangesAsync();
        }

        public IEnumerable<int> GetAllItemQuantities(int orderId)
        {
            return _orderRepository.GetAllItemQuantities(orderId);
        }

        public IEnumerable<Item> GetAllItems(int orderId)
        {
            return _orderRepository.GetAllItemsStored(orderId);
        }

        public IEnumerable<int> GetAllMenuQuantities(int orderId)
        {
            return _orderRepository.GetAllMenuQuantites(orderId);
        }

        public IEnumerable<Menu> GetAllMenus(int orderId)
        {
            return _orderRepository.GetAllMenus(orderId);
        }

        public IEnumerable<Order> GetAllWithReferences()
        {
            return _orderRepository.GetAllWithReferences();
        }

        public IEnumerable<Order> GetAllWithReferencesAsc()
        {
            return (_orderRepository.GetAllWithReferencesAsc());
        }

        public IEnumerable<Order> GetAllWithReferencesDesc()
        {
            return (_orderRepository.GetAllWithReferencesDesc());
        }

        public async Task Update(Order order)
        {
            await _orderRepository.Update(order);
        }
    }
}
