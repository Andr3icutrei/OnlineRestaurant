using Microsoft.Extensions.DependencyInjection;
using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Enums;
using OnlineRestaurant.UI.Models;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OnlineRestaurant.UI.ViewModel
{
    public class AdministrationWindowVM : INotifyPropertyChanged
    {
        #region Commands
        public ICommand InsertCommand { get; }
        public ICommand DisplayAllCommand { get; }
        public ICommand SelectionChangedCommand { get; }
        public ICommand DeleteRowCommand {  get; }
        public ICommand ModifyRowCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region CanCheckModifyAndDelete

        public bool IsModifyChecked { get; set; }
        public bool IsDeleteChecked { get; set; }
        #endregion

        #region Comboboxes references
        public ObservableCollection<string> ComboBoxItems { get; set; }
        public string InsertSelectedItem { get; set; } = null;
        public string ModifySelectedItem { get; set; } = null;
        public string DeleteSelectedItem { get; set; } = null;
        public string DisplayAllSelectedItem { get; set; } = null;
        public string AdminNameText { get; set; }
        #endregion

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
        
        private readonly INavigationService _navigationService;
        private readonly IItemService _itemService;
        private readonly IMenuService _menuService;
        private readonly IAllergenService _allergenService;
        private readonly IFoodCategoryService _foodCategoryService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public AdministrationWindowVM(INavigationService navigationService, IAllergenService allergenService,
            IItemService itemService,IMenuService menuService, IFoodCategoryService foodCategoryService)
        {
            _itemService = itemService;
            _menuService = menuService;
            _foodCategoryService = foodCategoryService;
            _allergenService = allergenService;
            _navigationService = navigationService;

            _currentColumns = new Dictionary<string, GridColumnDefinition>();

            CurrentGridData = new ObservableCollection<DataRowVM>();

            InsertCommand = new RelayCommand(Insert_Execute, Insert_CanExecute);
            DisplayAllCommand = new RelayCommand(DisplayAll_Execute, DisplayAll_CanExecute);
            DeleteRowCommand = new AsyncRelayCommand(Delete_Execute, Delete_CanExecute);
            ModifyRowCommand = new AsyncRelayCommand(Modify_Execute, Modify_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);

            ComboBoxItems = new ObservableCollection<string>()
            {
                "Categories", "Items", "Menus", "Allergens"
            };
        }

        public async Task LoadCategoriesAsync()
        {
            _currentColumns = new Dictionary<string, GridColumnDefinition>
            {
                ["Id"] = new GridColumnDefinition("Category ID", "Id", typeof(int)),
                ["FoodCategory"] = new GridColumnDefinition("Food Category", "Type", typeof(string))
            };

            List<FoodCategory> foodCategories = new List<FoodCategory>(await _foodCategoryService.GetAllAsync());

            CurrentGridData.Clear();
            foreach (var foodCategory in foodCategories)
            {
                CurrentGridData.Add(new DataRowVM(foodCategory, _currentColumns));
            }
        }

        public async Task LoadAllergensAsync()
        {
            _currentColumns = new Dictionary<string, GridColumnDefinition>
            {
                ["Id"] = new GridColumnDefinition("Allergen ID", "Id", typeof(int)),
                ["AllergenType"] = new GridColumnDefinition("Allergen Type", "Type", typeof(string))
            };

            List<Allergen> allergens = new List<Allergen>(await _allergenService.GetAllAsync());

            CurrentGridData.Clear();
            foreach (var allergen in allergens)
            {
                CurrentGridData.Add(new DataRowVM(allergen, _currentColumns));
            }
        }
        public async Task LoadItemsAsync()
        {
            _currentColumns = new Dictionary<string, GridColumnDefinition>
            {
                ["Id"] = new GridColumnDefinition("Item Id", "Id", typeof(int)),
                ["Name"] = new GridColumnDefinition("Name", "Name", typeof(string)),
                ["Price"] = new GridColumnDefinition("Price","Price",typeof(decimal)),
                ["PortionQuantity"] = new GridColumnDefinition("Portion Quantity", "PortionQuantity",typeof(float)),
                ["TotalQuantity"] = new GridColumnDefinition("Total Quantity", "TotalQuantity",typeof (float)),
                ["FoodCategoryId"] = new GridColumnDefinition("Food Category Id", "FoodCategory.Id",typeof(int)),
                ["FoodCategoryName"] = new GridColumnDefinition("Food Category Name", "FoodCategory.Type",typeof(string))
            };

            List<Item> items = new List<Item>(await _itemService.GetAllAsync());

            CurrentGridData.Clear();
            foreach (var item in items)
            {
                CurrentGridData.Add(new DataRowVM(item, _currentColumns));
            }
        }

        public async Task LoadMenusAsync()
        {
            _currentColumns = new Dictionary<string, GridColumnDefinition>
            {
                ["Id"] = new GridColumnDefinition("Menu Id", "Id", typeof(int)),
                ["Name"] = new GridColumnDefinition("Name", "Name", typeof(string)),
            };

            List<OnlineRestaurant.Database.Entities.Menu> menus = new List<OnlineRestaurant.Database.Entities.Menu>(await _menuService.GetAllAsync());

            CurrentGridData.Clear();
            foreach (var menu in menus)
            {
                CurrentGridData.Add(new DataRowVM(menu, _currentColumns));
            }
        }

        public void Insert_Execute()
        {
            if (InsertSelectedItem == "Categories")
                _navigationService.NavigateTo<UpsertFoodCategoryWindow>();
            else if (InsertSelectedItem == "Allergens")
                _navigationService.NavigateTo<UpsertAllergenWindow>();
            else if (InsertSelectedItem == "Items")
                _navigationService.NavigateTo<UpsertItemWindow>();
            else
                _navigationService.NavigateTo<UpsertMenuWindow>();
        }

        public async Task Delete_Execute()
        {
            if (DisplayAllSelectedItem == "Categories")
            {
                await _foodCategoryService.DeleteAsync(SelectedRow.GetOriginalData<FoodCategory>().Id);
                MessageBox.Show("Food category deleted succesfully", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (DisplayAllSelectedItem == "Items")
            {
                await _itemService.DeleteAsync(SelectedRow.GetOriginalData<Item>().Id);
                MessageBox.Show("Item deleted succesfully", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (DisplayAllSelectedItem == "Menus")
            {
                await _menuService.DeleteAsync(SelectedRow.GetOriginalData<OnlineRestaurant.Database.Entities.Menu>().Id);
                MessageBox.Show("Menu deleted succesfully", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            { 
                await _allergenService.DeleteAsync(SelectedRow.GetOriginalData<Allergen>().Id); 
                MessageBox.Show("Allergen deleted succesfully","Succes",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            DisplayAll_Execute();
        }

        public async Task Modify_Execute()
        {
            if (DisplayAllSelectedItem == "Allergens")
                _navigationService.NavigateTo<UpsertAllergenWindow>(SelectedRow.GetOriginalData<Allergen>());
            else if (DisplayAllSelectedItem == "Categories")
                _navigationService.NavigateTo<UpsertFoodCategoryWindow>(SelectedRow.GetOriginalData<FoodCategory>());
            else if (DisplayAllSelectedItem == "Items")
            {
                Item? currentItem = await _itemService.GetItemWithReferencesAsync(SelectedRow.GetOriginalData<Item>().Id);
                if (currentItem != null)
                { 
                    _navigationService.NavigateTo<UpsertItemWindow>(currentItem);
                }
            }
            else if(DisplayAllSelectedItem == "Menus")
            {
                OnlineRestaurant.Database.Entities.Menu? currentMenu = 
                    await _menuService.GetMenuWithReferencesAsync(SelectedRow.GetOriginalData<OnlineRestaurant.Database.Entities.Menu>().Id) ?? 
                    new OnlineRestaurant.Database.Entities.Menu();
                if(currentMenu != null)
                {
                    _navigationService.NavigateTo<UpsertMenuWindow>(currentMenu);
                }
            }
        }

        public async void DisplayAll_Execute()
        {
            if (DisplayAllSelectedItem == "Categories")
                await LoadCategoriesAsync();
            else if (DisplayAllSelectedItem == "Allergens")
                await LoadAllergensAsync();
            else if (DisplayAllSelectedItem == "Items")
                await LoadItemsAsync();
            else
                await LoadMenusAsync();

            Application.Current.Dispatcher.Invoke(() => {
                OnPropertyChanged(nameof(GridColumns));
                OnPropertyChanged(nameof(CurrentGridData));
            });
        }

        public bool Insert_CanExecute()
        {
            return InsertSelectedItem != null;
        }

        public bool Delete_CanExecute()
        {
            return SelectedRow != null;
        }

        public bool Modify_CanExecute()
        {
            return SelectedRow != null;
        }

        public bool DisplayAll_CanExecute()
        {
            return DisplayAllSelectedItem != null;
        }

        public void Cancel_Execute()
        {
            _navigationService.NavigateTo<StartupWindow>();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(propertyName, new PropertyChangedEventArgs(propertyName));
        }
    }
}
