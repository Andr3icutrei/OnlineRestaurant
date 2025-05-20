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
    public class GuestWindowVM : INotifyPropertyChanged
    {
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;
        private readonly IMenuService _menuService;
        private ICollectionView _groupedItems;
        private ObservableCollection<Item> _availableItems;
        private ObservableCollection<Item> _originalItems;
        private ObservableCollection<OnlineRestaurant.Database.Entities.Menu> _originalMenus;

        // Search properties
        private string _itemNameSearchText;
        private string _itemAllergenSearchText;
        private string _menuSearchText;
        private bool _isItemNameExcluded;
        private bool _isItemAllergenExcluded;
        private bool _isMenuNameExcluded;

        private Dictionary<int, int> _itemQuantities = new Dictionary<int, int>();
        private Dictionary<int, int> _menuQuantities = new Dictionary<int, int>();
        private User _currentUser;

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

        // Search by name property
        public string ItemNameSearchText
        {
            get => _itemNameSearchText;
            set
            {
                _itemNameSearchText = value;
                OnPropertyChanged();
            }
        }

        // Search by allergen property
        public string ItemAllergenSearchText
        {
            get => _itemAllergenSearchText;
            set
            {
                _itemAllergenSearchText = value;
                OnPropertyChanged();
            }
        }

        // Menu search property
        public string MenuSearchText
        {
            get => _menuSearchText;
            set
            {
                _menuSearchText = value;
                OnPropertyChanged();
            }
        }

        // Properties for "NOT" checkboxes
        public bool IsItemNameExcluded
        {
            get => _isItemNameExcluded;
            set
            {
                _isItemNameExcluded = value;
                OnPropertyChanged();
            }
        }

        public bool IsItemAllergenExcluded
        {
            get => _isItemAllergenExcluded;
            set
            {
                _isItemAllergenExcluded = value;
                OnPropertyChanged();
            }
        }

        public bool IsMenuNameExcluded
        {
            get => _isMenuNameExcluded;
            set
            {
                _isMenuNameExcluded = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public GuestWindowVM(IItemService itemService, IMenuService menuService, IOrderService orderService)
        {
            _itemService = itemService;
            _orderService = orderService;
            _menuService = menuService;

            // Initialize collections
            AvailableItems = new ObservableCollection<Item>();
            _originalItems = new ObservableCollection<Item>();
            AvailableMenus = new ObservableCollection<OnlineRestaurant.Database.Entities.Menu>();
            _originalMenus = new ObservableCollection<OnlineRestaurant.Database.Entities.Menu>();

            // Initialize search properties
            ItemNameSearchText = string.Empty;
            ItemAllergenSearchText = string.Empty;
            MenuSearchText = string.Empty;
            IsItemNameExcluded = false;
            IsItemAllergenExcluded = false;
            IsMenuNameExcluded = false;

            // Load data and setup UI
            InitializeAsync();
            UpdateGroupedItems();
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
                    _originalItems.Clear();
                    foreach (var item in items)
                    {
                        AvailableItems.Add(item);
                        _originalItems.Add(item);
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
                    _originalMenus.Clear();
                    foreach (var menu in menus)
                    {
                        AvailableMenus.Add(menu);
                        _originalMenus.Add(menu);
                    }
                    OnPropertyChanged(nameof(AvailableMenus));
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading items: {ex.Message}");
            }
        }

        private void UpdateGroupedItems()
        {
            var cvs = new CollectionViewSource { Source = AvailableItems };
            GroupedItems = cvs.View;
            GroupedItems.GroupDescriptions.Add(new PropertyGroupDescription("FoodCategory.Type"));
            GroupedItems.SortDescriptions.Add(new SortDescription("FoodCategory.Type", ListSortDirection.Ascending));
        }

        // Enhanced search methods for items
        public void SearchItems()
        {
            // If both search fields are empty, restore all items
            if (string.IsNullOrWhiteSpace(ItemNameSearchText) && string.IsNullOrWhiteSpace(ItemAllergenSearchText))
            {
                RestoreAllItems();
                return;
            }

            IEnumerable<Item> filteredItems = _originalItems;

            // Apply name filter if provided
            if (!string.IsNullOrWhiteSpace(ItemNameSearchText))
            {
                filteredItems = IsItemNameExcluded
                    ? filteredItems.Where(item => !item.Name.Contains(ItemNameSearchText, StringComparison.OrdinalIgnoreCase))
                    : filteredItems.Where(item => item.Name.Contains(ItemNameSearchText, StringComparison.OrdinalIgnoreCase));
            }

            // Apply allergen filter if provided
            if (!string.IsNullOrWhiteSpace(ItemAllergenSearchText))
            {
                filteredItems = IsItemAllergenExcluded
                    ? filteredItems.Where(item => !item.Allergens.Any(a => a.Type.Contains(ItemAllergenSearchText, StringComparison.OrdinalIgnoreCase)))
                    : filteredItems.Where(item => item.Allergens.Any(a => a.Type.Contains(ItemAllergenSearchText, StringComparison.OrdinalIgnoreCase)));
            }

            // Update available items
            UpdateAvailableItems(filteredItems);
        }

        // Enhanced search method for menus
        public void SearchMenus()
        {
            if (string.IsNullOrWhiteSpace(MenuSearchText))
            {
                RestoreAllMenus();
                return;
            }

            // Filter menus based on search text and NOT option
            var filteredMenus = IsMenuNameExcluded
                ? _originalMenus.Where(menu => !menu.Name.Contains(MenuSearchText, StringComparison.OrdinalIgnoreCase) &&
                                              !menu.ItemConfigurations.Any(ic => ic.Item.Name.Contains(MenuSearchText, StringComparison.OrdinalIgnoreCase)))
                : _originalMenus.Where(menu => menu.Name.Contains(MenuSearchText, StringComparison.OrdinalIgnoreCase) ||
                                              menu.ItemConfigurations.Any(ic => ic.Item.Name.Contains(MenuSearchText, StringComparison.OrdinalIgnoreCase)));

            // Update available menus
            UpdateAvailableMenus(filteredMenus);
        }

        // Helper methods for updating collections
        private void RestoreAllItems()
        {
            AvailableItems.Clear();
            foreach (var item in _originalItems)
            {
                AvailableItems.Add(item);
            }
        }

        private void RestoreAllMenus()
        {
            AvailableMenus.Clear();
            foreach (var menu in _originalMenus)
            {
                AvailableMenus.Add(menu);
            }
            OnPropertyChanged(nameof(AvailableMenus));
        }

        private void UpdateAvailableItems(IEnumerable<Item> filteredItems)
        {
            AvailableItems.Clear();
            foreach (var item in filteredItems)
            {
                AvailableItems.Add(item);
            }
        }

        private void UpdateAvailableMenus(IEnumerable<OnlineRestaurant.Database.Entities.Menu> filteredMenus)
        {
            AvailableMenus.Clear();
            foreach (var menu in filteredMenus)
            {
                AvailableMenus.Add(menu);
            }
            OnPropertyChanged(nameof(AvailableMenus));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

