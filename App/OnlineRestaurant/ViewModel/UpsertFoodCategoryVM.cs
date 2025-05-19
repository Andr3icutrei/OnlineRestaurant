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
    public class UpsertFoodCategoryVM : INotifyPropertyChanged
    {
        private FoodCategory? _foodCategoryToModify;
        public FoodCategory? FoodCategoryToModify
        {
            get => _foodCategoryToModify;
            set
            {
                _foodCategoryToModify = value;
                InitModifyMode();
            }
        }
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

        public UpsertFoodCategoryVM(IFoodCategoryService foodCategoryService, INavigationService navigationService) 
        {
            CategoryNameText = string.Empty;

            _foodCategoryService = foodCategoryService;
            _navigationService = navigationService;

            AddCategoryCommand = new AsyncRelayCommand(UpsertCategory_Execute, UpsertCategory_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);
        }

        private void InitModifyMode()
        {
            if (FoodCategoryToModify != null)
            {
                CategoryNameText = FoodCategoryToModify.Type;
            }
        }

        public async Task UpsertCategory_Execute()
        {
            if (FoodCategoryToModify == null)
            {
                FoodCategory category = new Database.Entities.FoodCategory() { Type = CategoryNameText };
                if (await _foodCategoryService.IsTypeUniqueAsync(category))
                {
                    await _foodCategoryService.AddFoodCategoryAsync(category);
                    MessageBox.Show("Food category added to the database!","Success",MessageBoxButton.OK,MessageBoxImage.Information);
                    ClearTextBoxes();
                }
                else
                {
                    MessageBox.Show("Food category already exists!","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            else
            {
                FoodCategoryToModify.Type = CategoryNameText;
                await _foodCategoryService.Update(FoodCategoryToModify);
                MessageBox.Show("Food category modified in the database!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Cancel_Execute() 
        {
            _navigationService.NavigateTo<AdministrationWindow>();
        }

        public bool UpsertCategory_CanExecute()
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
