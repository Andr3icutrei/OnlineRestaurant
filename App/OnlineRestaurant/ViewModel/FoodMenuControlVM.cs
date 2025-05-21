using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace OnlineRestaurant.UI.ViewModel
{
    public class FoodMenuControlVM: INotifyPropertyChanged
    {
        #region Private Fields
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;
        private readonly IMenuService _menuService;

        private ICollectionView _groupedItems;
        private ObservableCollection<Item> _availableItems;
        private Dictionary<int, int> _itemQuantities = new Dictionary<int, int>();
        private Dictionary<int, int> _menuQuantities = new Dictionary<int, int>();
        private User _currentUser;

        private decimal _orderTotal = 0;
        private ObservableCollection<OrderDisplay> _currentOrderfood = new ObservableCollection<OrderDisplay>();

        // Search properties
        private string _itemNameSearchText = "";
        private string _itemAllergenSearchText = "";
        private string _menuSearchText = "";
        private bool _isItemNameExcluded = false;
        private bool _isItemAllergenExcluded = false;
        private bool _isMenuNameExcluded = false;
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

        // Search properties with notification
        public string ItemNameSearchText
        {
            get { return _itemNameSearchText; }
            set
            {
                _itemNameSearchText = value;
                OnPropertyChanged();
            }
        }

        public string ItemAllergenSearchText
        {
            get { return _itemAllergenSearchText; }
            set
            {
                _itemAllergenSearchText = value;
                OnPropertyChanged();
            }
        }

        public string MenuSearchText
        {
            get { return _menuSearchText; }
            set
            {
                _menuSearchText = value;
                OnPropertyChanged();
            }
        }

        public bool IsItemNameExcluded
        {
            get { return _isItemNameExcluded; }
            set
            {
                _isItemNameExcluded = value;
                OnPropertyChanged();
            }
        }

        public bool IsItemAllergenExcluded
        {
            get { return _isItemAllergenExcluded; }
            set
            {
                _isItemAllergenExcluded = value;
                OnPropertyChanged();
            }
        }

        public bool IsMenuNameExcluded
        {
            get { return _isMenuNameExcluded; }
            set
            {
                _isMenuNameExcluded = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand IncrementCommand { get; }
        public ICommand DecrementCommand { get; }
        public ICommand PlaceOrderCommand { get; }
        public ICommand ClearOrderCommand { get; }
        public ICommand SearchItemsCommand { get; }
        public ICommand SearchMenusCommand { get; }
        #endregion

        public FoodMenuControlVM(IItemService itemService, IMenuService menuService, IOrderService orderService)
        {
            _itemService = itemService;
            _orderService = orderService;
            _menuService = menuService;

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
            SearchItemsCommand = new AsyncRelayCommand(SearchItems);
            SearchMenusCommand = new AsyncRelayCommand(SearchMenus);

            AvailableItems = new ObservableCollection<Item>();
            AvailableMenus = new ObservableCollection<OnlineRestaurant.Database.Entities.Menu>();
        }

        private async Task InitializeAsync()
        {
            await LoadItemsAsync();
            await LoadMenusAsync();
            UpdateGroupedItems();
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
                Debug.WriteLine($"Error loading menus: {ex.Message}");
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
                    itemToUpdate.TotalPrice = itemToUpdate.UnitPrice * itemToUpdate.Quantity;
                    OrderTotal += itemToUpdate.UnitPrice;
                }

                OnPropertyChanged(nameof(CurrentOrderFood));
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
                    itemToUpdate.TotalPrice = itemToUpdate.UnitPrice * itemToUpdate.Quantity;
                    OrderTotal -= itemToUpdate.UnitPrice;
                }

                OnPropertyChanged(nameof(CurrentOrderFood));
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
                    menuToUpdate.TotalPrice = menuToUpdate.UnitPrice * menuToUpdate.Quantity;
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
                    menuToUpdate.TotalPrice = menuToUpdate.UnitPrice * menuToUpdate.Quantity;
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
            if (_currentUser == null)
            {
                MessageBox.Show("No user is set. Cannot place order.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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
        }

        public async Task SetUser(User user)
        {
            _currentUser = user;
            await InitializeAsync();
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
            if (AvailableItems == null)
                return;

            var cvs = new CollectionViewSource { Source = AvailableItems };
            GroupedItems = cvs.View;

            if (GroupedItems.GroupDescriptions.Count == 0)
            {
                GroupedItems.GroupDescriptions.Add(new PropertyGroupDescription("FoodCategory.Type"));
                GroupedItems.SortDescriptions.Add(new SortDescription("FoodCategory.Type", ListSortDirection.Ascending));
            }
        }

        #region Search Methods
        public async Task SearchItems()
        {
            if (AvailableItems == null)
                return;

            var originalItems = new List<Item>(await _itemService.GetAllItemsWithReferencesAsync());
            AvailableItems.Clear();

            if (!string.IsNullOrWhiteSpace(ItemNameSearchText))
            {
                var nameFiltered = IsItemNameExcluded
                    ? originalItems.Where(i => !i.Name.Contains(ItemNameSearchText, StringComparison.OrdinalIgnoreCase))
                    : originalItems.Where(i => i.Name.Contains(ItemNameSearchText, StringComparison.OrdinalIgnoreCase));
                originalItems = nameFiltered.ToList();
            }

            if (!string.IsNullOrWhiteSpace(ItemAllergenSearchText))
            {
                var allergenFiltered = IsItemAllergenExcluded
                    ? originalItems.Where(i => !i.Allergens.Any(a => a.Type.Contains(ItemAllergenSearchText, StringComparison.OrdinalIgnoreCase)))
                    : originalItems.Where(i => i.Allergens.Any(a => a.Type.Contains(ItemAllergenSearchText, StringComparison.OrdinalIgnoreCase)));
                originalItems = allergenFiltered.ToList();
            }

            foreach (var item in originalItems)
            {
                AvailableItems.Add(item);
            }

            UpdateGroupedItems();
        }

        public async Task SearchMenus()
        {
            if (AvailableMenus == null)
                return;

            var originalMenus = new List<OnlineRestaurant.Database.Entities.Menu>(await _menuService.GetMenusWithReferences());
            AvailableMenus.Clear();

            if (!string.IsNullOrWhiteSpace(MenuSearchText))
            {
                var nameFiltered = IsMenuNameExcluded
                    ? originalMenus.Where(m => !m.Name.Contains(MenuSearchText, StringComparison.OrdinalIgnoreCase))
                    : originalMenus.Where(m => m.Name.Contains(MenuSearchText, StringComparison.OrdinalIgnoreCase));
                originalMenus = nameFiltered.ToList();
            }

            foreach (var menu in originalMenus)
            {
                AvailableMenus.Add(menu);
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
