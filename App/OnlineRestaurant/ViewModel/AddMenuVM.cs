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
using System.Windows.Controls;
using System.Windows.Input;

namespace OnlineRestaurant.UI.ViewModel
{
    public class AddMenuVM : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IFoodCategoryService _foodCategoryService;
        private readonly IItemService _itemService;

        public string MenuNameText { get; set; }
        public string MenuPortionQuantityText { get; set; }

        public ObservableCollection<FoodCategory> FoodCategoryItems { get; set; }
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
        public int SelectedFoodCategoryIndex { get; set; }
        public Dictionary<string, GridColumnDefinition> GridColumnsItems => _currentColumnsItems;

        private Dictionary<string, GridColumnDefinition> _currentColumnsItems;
        public ObservableCollection<DataRowVM> CurrentDataItems { get;set; }

        private ObservableCollection<DataRowVM> _selectedItems;
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
                    // Create a new collection with the items from the value
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

        public AddMenuVM(INavigationService navigationService,IItemService itemService,IFoodCategoryService foodCategoryService)
        {
            _foodCategoryService = foodCategoryService;
            _navigationService = navigationService;
            _itemService = itemService;

            AddMenuCommand = new RelayCommand(AddMenu_Execute, AddMenu_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);

            LoadFoodCategoryItems();
            LoadItemsDataGrid();
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

        public void AddMenu_Execute()
        {
            
        }

        public bool AddMenu_CanExecute()
        {
            return true;
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
