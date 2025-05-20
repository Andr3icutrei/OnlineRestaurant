using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Models;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace OnlineRestaurant.UI.ViewModel
{
    public class UserWindowVM : INotifyPropertyChanged
    {
        #region Private Fields
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;
        private readonly IMenuService _menuService;
        private readonly INavigationService _navigationService;

        private ICollectionView _groupedItems;
        private ObservableCollection<Item> _availableItems;
        private Dictionary<int, int> _itemQuantities = new Dictionary<int, int>();
        private Dictionary<int, int> _menuQuantities = new Dictionary<int, int>();
        private User _currentUser;
        private decimal _orderTotal = 0;
        private ObservableCollection<OrderDisplay> _currentOrderfood = new ObservableCollection<OrderDisplay>();

        // View switching fields
        private object _currentView;
        #endregion

        #region Properties
        public ICollectionView GroupedItems
        {
            get { return _groupedItems; }
            set
            {
                _groupedItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Item> AvailableItems
        {
            get { return _availableItems; }
            set
            {
                _availableItems = value;
                OnPropertyChanged();
                UpdateGroupedItems();
            }
        }

        public ObservableCollection<OnlineRestaurant.Database.Entities.Menu> AvailableMenus { get; set; }

        public decimal OrderTotal
        {
            get => _orderTotal;
            set
            {
                _orderTotal = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OrderDisplay> CurrentOrderFood
        {
            get => _currentOrderfood;
            set
            {
                _currentOrderfood = value;
                OnPropertyChanged();
            }
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand IncrementCommand { get; }
        public ICommand DecrementCommand { get; }
        public ICommand PlaceOrderCommand { get; }
        public ICommand ClearOrderCommand { get; }
        public ICommand CancelCommand { get; }

        // View switching commands
        public ICommand SwitchToFoodMenuViewCommand { get; }
        public ICommand SwitchToOrdersViewCommand { get; }
        #endregion

        public UserWindowVM(IItemService itemService, IMenuService menuService, IOrderService orderService,
            INavigationService navigationService)
        {
            _itemService = itemService;
            _orderService = orderService;
            _menuService = menuService;
            _navigationService = navigationService;

            IncrementCommand = new RelayCommandWithParameter(
                param =>
                {
                    if (param is Item item)
                    {
                        IncrementQuantity(item);
                    }
                    else if (param is OnlineRestaurant.Database.Entities.Menu menu)
                    {
                        IncrementQuantity(menu);
                    }
                },
                param => param is Item || param is OnlineRestaurant.Database.Entities.Menu
            );

            DecrementCommand = new RelayCommandWithParameter(
                param =>
                {
                    if (param is Item item)
                    {
                        DecrementQuantity(item);
                    }
                    else if (param is OnlineRestaurant.Database.Entities.Menu menu)
                    {
                        DecrementQuantity(menu);
                    }
                },
                param => param is Item || param is OnlineRestaurant.Database.Entities.Menu
            );

            PlaceOrderCommand = new AsyncRelayCommand(PlaceOrder, CanPlaceOrder);
            ClearOrderCommand = new RelayCommand(ClearOrder);
            CancelCommand = new RelayCommand(Cancel_Execute);

            SwitchToFoodMenuViewCommand = new RelayCommand(SwitchToFoodMenuView);
            SwitchToOrdersViewCommand = new RelayCommand(SwitchToOrdersView);

            AvailableItems = new ObservableCollection<Item>();
            AvailableMenus = new ObservableCollection<OnlineRestaurant.Database.Entities.Menu>();

            InitializeAsync();
            UpdateGroupedItems();

            SwitchToFoodMenuView();
        }

        public void ConfigureInit(User user)
        {
            _currentUser = user;
        }

        private async void InitializeAsync()
        {
            await LoadItemsAsync();
            await LoadMenusAsync();
        }

        public async Task LoadItemsAsync()
        {
            try
            {
                var items = await _itemService.GetAllItemsWithReferencesAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    AvailableItems.Clear();
                    foreach (var item in items)
                    {
                        AvailableItems.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading items: {ex.Message}");
            }
        }

        public async Task LoadMenusAsync()
        {
            try
            {
                List<OnlineRestaurant.Database.Entities.Menu>? menus = new List<Database.Entities.Menu>(
                    await _menuService.GetMenusWithReferences());

                Application.Current.Dispatcher.Invoke(() =>
                {
                    AvailableMenus.Clear();
                    foreach (var menu in menus)
                    {
                        AvailableMenus.Add(menu);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading items: {ex.Message}");
            }
        }

        private void IncrementQuantity(Item item)
        {
            if (item != null)
            {
                if (!_itemQuantities.ContainsKey(item.Id))
                {
                    _itemQuantities[item.Id] = 0;
                    CurrentOrderFood.Add(new OrderDisplay
                    {
                        IsItem = true,
                        Id = item.Id,
                        Name = item.Name,
                        Quantity = 1,
                        UnitPrice = item.Price,
                        TotalPrice = item.Price
                    });

                    OrderTotal += item.Price;
                    _itemQuantities[item.Id]++;
                    return;
                }

                _itemQuantities[item.Id]++;

                OrderDisplay? itemToUpdate = CurrentOrderFood.FirstOrDefault(coi => coi.Id == item.Id && coi.IsItem) ?? null;
                if (itemToUpdate != null)
                {
                    itemToUpdate.Quantity++;
                    OrderTotal += itemToUpdate.UnitPrice;
                }

                OnPropertyChanged(nameof(CurrentOrderFood));
            }
            foreach (var quantity in _itemQuantities)
            {
                Debug.Print($"item {quantity.Key}: quantity {quantity.Value}");
            }
        }

        private void DecrementQuantity(Item item)
        {
            if (item != null && _itemQuantities.ContainsKey(item.Id))
            {
                if (_itemQuantities[item.Id] > 1)
                {
                    _itemQuantities[item.Id]--;
                }
                else
                {
                    for (int i = 0; i < CurrentOrderFood.Count; i++)
                    {
                        if (CurrentOrderFood[i].Id == item.Id && CurrentOrderFood[i].IsItem)
                        {
                            OrderTotal -= CurrentOrderFood[i].UnitPrice;
                            CurrentOrderFood.RemoveAt(i);
                            _itemQuantities.Remove(item.Id);
                            return;
                        }
                    }
                }

                OrderDisplay? itemToUpdate = CurrentOrderFood.FirstOrDefault(coi => coi.Id == item.Id && coi.IsItem) ?? null;
                if (itemToUpdate != null)
                {
                    itemToUpdate.Quantity--;
                    OrderTotal -= itemToUpdate.UnitPrice;
                }

                OnPropertyChanged(nameof(CurrentOrderFood));
            }
            foreach (var quantity in _itemQuantities)
            {
                Debug.Print($"item {quantity.Key}: quantity {quantity.Value}");
            }
        }

        private async Task IncrementQuantity(OnlineRestaurant.Database.Entities.Menu menu)
        {
            if (menu != null)
            {
                if (!_menuQuantities.ContainsKey(menu.Id))
                {
                    decimal menuPrice = await _menuService.CalculateMenuPriceAsync(menu.Id);
                    _menuQuantities[menu.Id] = 0;
                    CurrentOrderFood.Add(new OrderDisplay
                    {
                        IsItem = false,
                        Id = menu.Id,
                        Name = menu.Name,
                        Quantity = 1,
                        UnitPrice = menuPrice,
                        TotalPrice = menuPrice
                    });

                    OrderTotal += menuPrice;
                    _menuQuantities[menu.Id]++;
                    return;
                }

                _menuQuantities[menu.Id]++;
                OrderDisplay? menuToUpdate = CurrentOrderFood.FirstOrDefault(coi => coi.Id == menu.Id && !coi.IsItem) ?? null;
                if (menuToUpdate != null)
                {
                    menuToUpdate.Quantity++;
                    OrderTotal += menuToUpdate.UnitPrice;
                }

                OnPropertyChanged(nameof(CurrentOrderFood));
            }
        }

        private void DecrementQuantity(OnlineRestaurant.Database.Entities.Menu menu)
        {
            if (menu != null && _menuQuantities.ContainsKey(menu.Id))
            {
                if (_menuQuantities[menu.Id] > 1)
                {
                    _menuQuantities[menu.Id]--;
                }
                else
                {
                    for (int i = 0; i < CurrentOrderFood.Count; i++)
                    {
                        if (CurrentOrderFood[i].Id == menu.Id && !CurrentOrderFood[i].IsItem)
                        {
                            OrderTotal -= CurrentOrderFood[i].UnitPrice;
                            CurrentOrderFood.RemoveAt(i);
                            _menuQuantities.Remove(menu.Id);
                            return;
                        }
                    }
                }

                OrderDisplay? menuToUpdate = CurrentOrderFood.FirstOrDefault(coi => coi.Id == menu.Id && !coi.IsItem) ?? null;
                if (menuToUpdate != null)
                {
                    menuToUpdate.Quantity--;
                    OrderTotal -= menuToUpdate.UnitPrice;
                }

                OnPropertyChanged(nameof(CurrentOrderFood));
            }
        }

        private bool CanPlaceOrder()
        {
            return CurrentOrderFood.Count > 0;
        }

        private async Task PlaceOrder()
        {
            var order = new Order
            {
                State = Database.Enums.OrderState.Registered,
                Items = new List<ItemOrder>(),
                Menus = new List<MenuOrder>(),
                User = _currentUser,
                Price = CurrentOrderFood.Sum(cof => cof.TotalPrice),
            };

            foreach (var orderItem in CurrentOrderFood)
            {
                if (orderItem.IsItem)
                {
                    Item? itemToAdd = await _itemService.GetItemWithReferencesAsync(orderItem.Id);
                    if (itemToAdd != null)
                        order.Items.Add(new ItemOrder
                        {
                            ItemsId = itemToAdd.Id,
                            OrdersId = order.Id,
                            NoItems = orderItem.Quantity
                        });
                }
                else
                {
                    OnlineRestaurant.Database.Entities.Menu? menuToAdd = await _menuService.GetMenuWithReferencesAsync(orderItem.Id);
                    if (menuToAdd != null)
                        order.Menus.Add(new MenuOrder
                        {
                            MenusId = menuToAdd.Id,
                            OrdersId = order.Id,
                            NoMenus = orderItem.Quantity
                        });
                }
            }

            await _orderService.AddOrder(order);

            MessageBox.Show($"Order placed successfully!",
                "Order Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearOrder();

            SwitchToOrdersView();
        }

        private void ClearOrder()
        {
            _itemQuantities.Clear();
            _menuQuantities.Clear();

            CurrentOrderFood.Clear();
            OrderTotal = 0;
        }

        private void UpdateGroupedItems()
        {
            var cvs = new CollectionViewSource { Source = AvailableItems };
            GroupedItems = cvs.View;

            GroupedItems.GroupDescriptions.Add(new PropertyGroupDescription("FoodCategory.Type"));
            GroupedItems.SortDescriptions.Add(new SortDescription("FoodCategory.Type", ListSortDirection.Ascending));
        }

        #region View Switching Methods
        private void SwitchToFoodMenuView()
        {
            var foodMenuControl = new FoodMenuUserControl
            {
                DataContext = this
            };

            CurrentView = foodMenuControl;
        }

        private void SwitchToOrdersView()
        {
            OrdersControl ordersControl = new OrdersControl(_currentUser);

            CurrentView = ordersControl;
        }

        private void Cancel_Execute()
        {
            _navigationService.NavigateTo<StartupWindow>();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}