using Microsoft.EntityFrameworkCore.Metadata;
using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Models;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OnlineRestaurant.UI.ViewModel
{
    public class UpsertMenuVM : INotifyPropertyChanged
    {
        private OnlineRestaurant.Database.Entities.Menu _menuToUpdate;
        public OnlineRestaurant.Database.Entities.Menu MenuToUpdate
        {
            get => _menuToUpdate;
            set
            {
                _menuToUpdate = value;
                InitUpdateMode();
            }
        }

        private readonly INavigationService _navigationService;
        private readonly IFoodCategoryService _foodCategoryService;
        private readonly IItemService _itemService;
        private readonly IMenuService _menuService;
        private readonly IMenuItemConfigurationService _menuItemConfigurationService;

        public string MenuNameText { get; set; }
        public ObservableCollection<FoodCategory> FoodCategoryItems { get; set; }
        public int SelectedFoodCategoryIndex { get; set; } = -1;
        private FoodCategory _selectedFoodCategory;
        public FoodCategory SelectedFoodCategory
        {
            get => _selectedFoodCategory;
            set
            {
                _selectedFoodCategory = value;
                OnPropertyChanged(nameof(SelectedFoodCategory));
                SelectedFoodCategoryIndex = FoodCategoryItems.IndexOf(value) + 1;
            }
        }
        public IEnumerable<KeyValuePair<string, GridColumnDefinition>> GridColumnsItems => _currentColumnsItems;

        private Dictionary<string, GridColumnDefinition> _currentColumnsItems;
        public ObservableCollection<DataRowVM> CurrentDataItems { get; set; }

        private ObservableCollection<DataRowVM> _selectedItems = new ObservableCollection<DataRowVM>();
        public IList SelectedItems
        {
            get => _selectedItems;
            set
            {
                if (value is ObservableCollection<DataRowVM> collection)
                {
                    _selectedItems = collection;
                }
                else if (value != null)
                {
                    _selectedItems = new ObservableCollection<DataRowVM>(value.Cast<DataRowVM>());
                }
                else
                {
                    _selectedItems = new ObservableCollection<DataRowVM>();
                }
                OnPropertyChanged(nameof(SelectedItems));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddMenuCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public UpsertMenuVM(INavigationService navigationService,IItemService itemService,IFoodCategoryService foodCategoryService,
            IMenuService menuService,IMenuItemConfigurationService menuItemConfigurationService)
        {
            _menuItemConfigurationService = menuItemConfigurationService;
            _menuService = menuService;
            _foodCategoryService = foodCategoryService;
            _navigationService = navigationService;
            _itemService = itemService;

            AddMenuCommand = new AsyncRelayCommand(UpsertMenu_Execute, UpsertMenu_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);

            LoadFoodCategoryItems();
            LoadItemsDataGrid();
        }

        private async Task InitUpdateMode()
        {
            MenuNameText = MenuToUpdate.Name;
            SelectedFoodCategoryIndex = MenuToUpdate.FoodCategoryId - 1 ?? 0;

            _selectedItems.Clear();

            List<Item> items = new List<Item>(await _menuService.GetItemsForMenuAsync(MenuToUpdate.Id));

            foreach(Item item in items)
            {
                _selectedItems.Add(new DataRowVM(item,_currentColumnsItems));
            }
        }

        public void LoadFoodCategoryItems()
        {
            FoodCategoryItems = new ObservableCollection<FoodCategory>(_foodCategoryService.GetAll().ToList());
        }

        public void LoadItemsDataGrid()
        {
            _currentColumnsItems = new Dictionary<string, GridColumnDefinition>
            {
                ["Id"] = new GridColumnDefinition("Item id", "Id", typeof(string)),
                ["ItemName"] = new GridColumnDefinition("Name", "Name", typeof(string)),
                ["PortionQuantity"] = new GridColumnDefinition("Portion Quantity", "PortionQuantity", typeof(float))
            };

            CurrentDataItems = new ObservableCollection<DataRowVM>();

            List<Item> items = _itemService.GetAll().ToList();

            foreach(var item in items)
            {
                CurrentDataItems.Add(new DataRowVM(item, _currentColumnsItems));
            }
        }

        public async Task UpsertMenu_Execute()
        {
            WindowService ws = new WindowService();
            List<Item> items = new List<Item>();
            for(int i = 0; i < _selectedItems.Count; i++)
            {
                items.Add(_selectedItems[i].GetOriginalData<Item>());
            }

            ws.ShowVariableTextBoxesWindow(
            items,
            this,
            async (vm, wasConfirmed) =>
                {
                    if(!wasConfirmed)
                        return;

                    var viewModel = (VariableTextBoxesVM)vm;

                    if (MenuToUpdate == null)
                    {
                        Database.Entities.Menu menu = new Database.Entities.Menu
                        {
                            Name = MenuNameText,
                            FoodCategoryId = SelectedFoodCategoryIndex
                        };

                        await _menuService.AddMenuAsync(menu);

                        for (int i = 0; i < viewModel.TextFieldItems.Count; i++)
                        {
                            MenuItemConfiguration menuItemConfiguration = new MenuItemConfiguration()
                            {
                                ItemId = _selectedItems[i].GetOriginalData<Item>().Id,
                                MenuId = menu.Id,
                                MenuPortionQuantity = float.Parse(viewModel.TextFieldItems[i].Value)
                            };
                            await _menuItemConfigurationService.AddMenuItemConfigurationAsync(menuItemConfiguration);
                        }

                        MessageBox.Show("Menu added to the database!","Success",MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MenuToUpdate.Name = MenuNameText;
                        MenuToUpdate.FoodCategoryId = SelectedFoodCategoryIndex;

                        await _menuService.UpdateAsync(MenuToUpdate);

                        int i = 0;
                        foreach(MenuItemConfiguration configuration in MenuToUpdate.ItemConfigurations) 
                        {
                            configuration.ItemId = _selectedItems[i].GetOriginalData<Item>().Id;
                            configuration.MenuId = MenuToUpdate.Id;
                            configuration.MenuPortionQuantity = float.Parse(viewModel.TextFieldItems[i].Value);
                            await _menuItemConfigurationService.UpdateAsync(configuration);
                            i++;
                        };

                        MessageBox.Show("Menu modified in the database!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
        );
        }

        public bool UpsertMenu_CanExecute()
        {
            return MenuNameText != string.Empty && SelectedFoodCategoryIndex != -1;
        }

        public void Cancel_Execute() 
        {
            _navigationService.NavigateTo<AdministrationWindow>();
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(propertyName, new PropertyChangedEventArgs(propertyName));
        }
    }
}
