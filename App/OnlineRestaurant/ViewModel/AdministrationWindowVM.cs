using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Enums;
using OnlineRestaurant.UI.Models;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnlineRestaurant.UI.ViewModel
{
    public class AdministrationWindowVM : INotifyPropertyChanged
    {
        #region Commands
        public ICommand InsertCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ModifyCommand { get; }
        public ICommand DisplayAllCommand { get; }
        #endregion

        #region Comboboxes references
        public ObservableCollection<string> ComboBoxItems { get; set; }
        public string InsertSelectedItem { get; set; } = null;
        public string ModifySelectedItem { get; set; } = null;
        public string DeleteSelectedItem { get; set; } = null;
        public string DisplayAllSelectedItem { get; set; } = null;
        #endregion

        public ObservableCollection<DataRowVM> CurrentData { get; set; }
        private Dictionary<string, GridColumnDefinition> _currentColumns;

        private readonly INavigationService _navigationService; 

        public event PropertyChangedEventHandler? PropertyChanged;

        public AdministrationWindowVM(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InsertCommand = new RelayCommand(Insert_Execute, Insert_CanExecute);
            DeleteCommand = new RelayCommand(Delete_Execute, Delete_CanExecute);
            ModifyCommand = new RelayCommand(Modify_Execute, Modify_CanExecute);
            DisplayAllCommand = new RelayCommand(DisplayAll_Execute, DisplayAll_CanExecute);

            ComboBoxItems = new ObservableCollection<string>()
            {
                "Categories", "Items", "Menus", "Allergens"
            };
        }

        public async void LoadCategoriesAsync()
        {
            _currentColumns = new Dictionary<string, GridColumnDefinition>
            {
                ["Id"] = new GridColumnDefinition("Category ID", "Id", typeof(int)),
                ["FoodCategory"] = new GridColumnDefinition("Food Category", "Type", typeof(string))
            };
        }

        public async void LoadItemsAsync()
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
        }

        public async void LoadMenusAsync()
        {
            _currentColumns = new Dictionary<string, GridColumnDefinition>
            {
                ["Id"] = new GridColumnDefinition("Menu Id", "Id", typeof(int)),
                ["FoodCategoryId"] = new GridColumnDefinition("Food Category Id", "FoodCategory.Id", typeof(int)),
                ["Items"] = new GridColumnDefinition("Componsing items", "Items.Name", typeof(string)),
                ["Orders"] = new GridColumnDefinition("Total Orders", "Orders.Id", typeof(int)),
            };

            //var items = await 
        }

        public void Insert_Execute()
        {
            if (InsertSelectedItem == "Categories")
                _navigationService.NavigateTo<AddFoodCategoryWindow>();
            else if (InsertSelectedItem == "Allergens")
                _navigationService.NavigateTo<AddAllergenWindow>();
            else if (InsertSelectedItem == "Items")
                _navigationService.NavigateTo<AddItemWindow>();
        }

        public void Delete_Execute()
        {

        }

        public void Modify_Execute()
        {

        }

        public void DisplayAll_Execute()
        {

        }

        public bool Insert_CanExecute()
        {
            return InsertSelectedItem != null;
        }

        public bool Delete_CanExecute()
        {
            return DeleteSelectedItem != null;
        }

        public bool Modify_CanExecute()
        {
            return ModifySelectedItem != null;
        }

        public bool DisplayAll_CanExecute()
        {
            return DisplayAllSelectedItem != null;
        }
    }
}
