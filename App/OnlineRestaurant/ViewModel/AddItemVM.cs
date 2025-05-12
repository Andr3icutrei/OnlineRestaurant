using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Models;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.View;
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
    public class AddItemVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _portionQuantityText;
        private string _totalQuantityText;
        private string _itemNameText;
        private string _priceText;

        public string PriceText
        {
            get => _priceText;
            set
            {
                _priceText = value;
                OnPropertyChanged(nameof(PriceText));
            }
        }
        public string ItemNameText
        {
            get => _itemNameText;
            set
            {
                _itemNameText = value;
                OnPropertyChanged(nameof(ItemNameText));
            }
        }
        public string PortionQuantityText
        {
            get => _portionQuantityText;
            set
            {
                _portionQuantityText = value;
                OnPropertyChanged(nameof(PortionQuantityText));
            }
        }
        public string TotalQuantityText
        {
            get => _totalQuantityText;
            set
            {
                _totalQuantityText = value;
                OnPropertyChanged(nameof(TotalQuantityText));
            }
        }

        public ObservableCollection<FoodCategory> FoodCategoryItems { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly INavigationService _navigationService;
        private readonly IItemService _itemService;
        private readonly IAllergenService _allergenService;

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

        public IEnumerable<KeyValuePair<string, GridColumnDefinition>> GridColumnsItemPicture => _currentColumnsItemPictures;

        private ObservableCollection<DataRowVM> _currentDataItemPictures;
        public ObservableCollection<DataRowVM> CurrentDataItemPictures
        {
            get => _currentDataItemPictures;
            set
            {
                _currentDataItemPictures = value;
                OnPropertyChanged(nameof(CurrentDataItemPictures));
            }
        }
        private Dictionary<string, GridColumnDefinition> _currentColumnsItemPictures;
        public Dictionary<string, GridColumnDefinition> CurrentColumnsItemPictures
        {
            get => _currentColumnsItemPictures;
            set
            {
                _currentColumnsItemPictures = value;
                // This will trigger the UI to update the columns
                OnPropertyChanged(nameof(CurrentColumnsItemPictures));
            }
        }

        public IEnumerable<KeyValuePair<string, GridColumnDefinition>> GridColumnsAllergens => _currentColumnsAllergens;

        private ObservableCollection<DataRowVM> _currentDataAllergens;
        public ObservableCollection<DataRowVM> CurrentDataAllergens
        {
            get => _currentDataAllergens;
            set
            {
                _currentDataAllergens = value;
                OnPropertyChanged(nameof(CurrentDataAllergens));
            }
        }
        private Dictionary<string, GridColumnDefinition> _currentColumnsAllergens;
        public Dictionary<string, GridColumnDefinition> CurrentColumnsAllergens
        {
            get => _currentColumnsAllergens;
            set
            {
                _currentColumnsAllergens = value;
                // This will trigger the UI to update the columns
                OnPropertyChanged(nameof(CurrentColumnsAllergens));
            }
        }

        public int SelectedFoodCategoryIndex { get; set; }

        public ICommand AddItemCommand { get; }
        public ICommand CancelCommand { get; }

        public AddItemVM(INavigationService navigationService,IItemService itemService,IFoodCategoryService categoryService,
            IAllergenService allergenService)
        {
            _navigationService = navigationService;
            _itemService = itemService;
            _allergenService = allergenService;

            AddItemCommand = new RelayCommand(AddItem_Execute, AddItem_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);

            FoodCategoryItems = new ObservableCollection<FoodCategory>(categoryService.GetAll());
            LoadItemPictures();
            LoadAllergens();
        }

        public void LoadItemPictures()
        {
            _currentColumnsItemPictures = new Dictionary<string, GridColumnDefinition>()
            {
                ["Id"] = new GridColumnDefinition("Picture Id", "Id", typeof(int)),
                ["PicturePath"] = new GridColumnDefinition("Picture path", "PicturePath", typeof(string))
            };

            CurrentDataItemPictures = new ObservableCollection<DataRowVM>();
            var item1 = new ItemPicture { Id = 1, PicturePath = "path/to/image1.jpg" };
            var item2 = new ItemPicture { Id = 2, PicturePath = "path/to/image2.jpg" };
            CurrentDataItemPictures.Add(new DataRowVM(item1, CurrentColumnsItemPictures));
            CurrentDataItemPictures.Add(new DataRowVM(item2, CurrentColumnsItemPictures));
        }

        public void LoadAllergens()
        {
            _currentColumnsAllergens = new Dictionary<string, GridColumnDefinition>()
            {
                ["Id"] = new GridColumnDefinition("Allergen Id", "Id", typeof(int)),
                ["AllergenType"] = new GridColumnDefinition("Allergen Type", "Type", typeof(string))
            };

            CurrentDataAllergens = new ObservableCollection<DataRowVM>();

            List<Allergen> allergens = _allergenService.GetAll().ToList();

            foreach(var allergen in allergens)
            {
                CurrentDataAllergens.Add(new DataRowVM((allergen), CurrentColumnsAllergens));
            }
        }


        public async void AddItem_Execute()
        {
            try
            {
                Item i = new Item()
                {
                    Name = ItemNameText,
                    Price = decimal.Parse(PriceText),
                    PortionQuantity = float.Parse(PortionQuantityText),
                    TotalQuantity = float.Parse(TotalQuantityText),
                    FoodCategoryId = SelectedFoodCategoryIndex
                };
                await _itemService.AddItemAsync(i);
            } 
            catch (FormatException e)
            {
                MessageBox.Show("You did not insert a valid number for price or quantity");  
            }
        }

        public bool AddItem_CanExecute()
        {
            return ItemNameText != string.Empty && PortionQuantityText != string.Empty && 
                TotalQuantityText != string.Empty && PriceText != string.Empty;
        }
        public void Cancel_Execute()
        {
            _navigationService.NavigateTo<AdministrationWindow>();
        }
    }
}
