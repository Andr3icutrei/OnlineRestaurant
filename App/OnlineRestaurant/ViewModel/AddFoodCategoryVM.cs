using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OnlineRestaurant.UI.ViewModel
{
    public class AddFoodCategoryVM : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IFoodCategoryService _foodCategoryService;

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _categoryNameText;
        public string CategoryNameText
        {
            get => _categoryNameText;
            set
            {
                _categoryNameText = value;
                OnPropertyChanged(nameof(CategoryNameText));
            }
        }
        public ObservableCollection<string> ComboBoxItems { get; set; }

        public ICommand AddCategoryCommand { get; }
        public ICommand CancelCommand { get; }

        public AddFoodCategoryVM(IFoodCategoryService foodCategoryService, INavigationService navigationService) 
        {
            CategoryNameText = string.Empty;

            _foodCategoryService = foodCategoryService;
            _navigationService = navigationService;

            AddCategoryCommand = new RelayCommand(AddCategory_Execute,AddCategory_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);
        }

        public async void AddCategory_Execute()
        {
            FoodCategory category = new Database.Entities.FoodCategory() { Type = CategoryNameText };
            if (await _foodCategoryService.IsTypeUniqueAsync(category))
            { 
                _foodCategoryService.AddFoodCategoryAsync(category);
                MessageBox.Show("Food category added!");
                ClearTextBoxes();
            }
            else
            {
                MessageBox.Show("Food category already exists!");
            }
        }

        public void Cancel_Execute() 
        {
            _navigationService.NavigateTo<AdministrationWindow>();
        }

        public bool AddCategory_CanExecute()
        {
            return CategoryNameText != string.Empty;
        }

        private void ClearTextBoxes()
        {
            CategoryNameText = string.Empty;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
