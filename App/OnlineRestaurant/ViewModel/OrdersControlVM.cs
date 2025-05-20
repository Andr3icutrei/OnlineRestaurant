using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Enums;
using OnlineRestaurant.UI.Models;
using OnlineRestaurant.UI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OnlineRestaurant.UI.ViewModel
{
    public class OrdersControlVM : INotifyPropertyChanged
    {
        private readonly IOrderService _orderService;
        public User CurrentUser { get; set; }
        private List<Order> _orders = new List<Order>();
        public IEnumerable<KeyValuePair<string, GridColumnDefinition>> GridColumns => _currentColumns;

        private Dictionary<string, GridColumnDefinition> _currentColumns;
        private ObservableCollection<DataRowVM> _currentGridData;
        public ObservableCollection<DataRowVM> CurrentGridData
        {
            get => _currentGridData;
            set
            {
                _currentGridData = value;
                OnPropertyChanged(nameof(CurrentGridData));
            }
        }

        public DataRowVM SelectedRow { get; set; }

        public List<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }

        public ICommand ViewOrderDetailsCommand { get; }
        public ICommand CancelOrderCommand { get; }

        public OrdersControlVM(IOrderService orderService)
        {
            _orderService = orderService;


            CancelOrderCommand = new AsyncRelayCommand(CancelOrder_Execute,CancelOrder_CanExecute);
            LoadOrders();
        }

        public void LoadOrders()
        {
            _orders = new List<Order>(_orderService.GetAllWithReferences());
            List<string> itemsDescription = new List<string>();
            List<string> menusDescription = new List<string>();

            foreach (Order order in _orders)
            {
                var itemNames = _orderService.GetAllItems(order.Id).Select(item => item.Name).ToList();
                var itemsQuantities = _orderService.GetAllItemQuantities(order.Id).ToList();

                var menuNames = _orderService.GetAllMenus(order.Id).Select(menu => menu.Name).ToList();
                var menusQuantities = _orderService.GetAllMenuQuantities(order.Id).ToList();

                var itemDisplayList = itemNames.Zip(itemsQuantities, (name, qty) => $"{name} x{qty}");
                var menuDisplayList = menuNames.Zip(menusQuantities, (name, qty) => $"{name} x{qty}");

                string itemsString = string.Join("\n ", itemDisplayList);
                string menusString = string.Join("\n ", menuDisplayList);

                itemsDescription.Add(itemsString);
                menusDescription.Add(menusString);
            }

            List<OrderDisplayDataGrid> orderDisplays = new List<OrderDisplayDataGrid>();
            for(int i = 0;i < _orders.Count;i++)
            {
                orderDisplays.Add(new OrderDisplayDataGrid(_orders[i].Id, _orders[i].Price, _orders[i].State, itemsDescription[i], menusDescription[i]));
            }

            SelectedRow = null;
            CurrentGridData = new ObservableCollection<DataRowVM>();
            _currentColumns = new Dictionary<string, GridColumnDefinition>();

            _currentColumns = new Dictionary<string, GridColumnDefinition>
            {
                ["Id"] = new GridColumnDefinition("Order Id", "Id", typeof(int)),
                ["Price"] = new GridColumnDefinition("Price", "Price", typeof(decimal)),
                ["State"] = new GridColumnDefinition("State", "State", typeof(OrderState)),
                ["Items"] = new GridColumnDefinition("Items", "ItemDescription", typeof(string)),
                ["Menus"] = new GridColumnDefinition("Menus", "MenuDescription", typeof(string))
            };

            foreach (OrderDisplayDataGrid orderDisplay in orderDisplays)
            {
                CurrentGridData.Add(new DataRowVM(orderDisplay,_currentColumns));
            }
        }

        public async Task CancelOrder_Execute()
        {
            
        }

        public bool CancelOrder_CanExecute()
        {
            return SelectedRow != null;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
