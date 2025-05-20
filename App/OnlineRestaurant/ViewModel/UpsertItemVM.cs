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
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OnlineRestaurant.UI.ViewModel
{
    public class UpsertItemVM : INotifyPropertyChanged
    {
        private Item _itemToModify;
        public Item ItemToModify
        {
            get => _itemToModify;
            set
            {
                _itemToModify = value;
                InitModifyMode();
            }
        }

        private readonly INavigationService _navigationService;
        private readonly IItemService _itemService;
        private readonly IAllergenService _allergenService;
        private readonly IItemPictureService _itemPictureService;


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

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<FoodCategory> FoodCategoryItems { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
                OnPropertyChanged(nameof(CurrentColumnsAllergens));
            }
        }

        public int SelectedFoodCategoryIndex { get; set; }

        public IEnumerable<KeyValuePair<string, GridColumnDefinition>> GridColumnsItemPictures => _currentColumnsItemPictures;

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
                OnPropertyChanged(nameof(CurrentColumnsItemPictures));
            }
        }

        private ObservableCollection<DataRowVM> _selectedItemPictures = new ObservableCollection<DataRowVM>();
        public IList SelectedItemPictures
        {
            get => _selectedItemPictures;
            set
            {
                if (value is ObservableCollection<DataRowVM> collection)
                {
                    _selectedItemPictures = collection;
                }
                else if (value != null)
                {
                    // Create a new collection with the items from the value
                    _selectedItemPictures = new ObservableCollection<DataRowVM>(value.Cast<DataRowVM>());
                }
                else
                {
                    _selectedItemPictures = new ObservableCollection<DataRowVM>();
                }
                OnPropertyChanged(nameof(SelectedItemPictures));
            }
        }

        private ObservableCollection<DataRowVM> _selectedAllergens = new ObservableCollection<DataRowVM>();
        public IList SelectedAllergens
        {
            get => _selectedAllergens;
            set
            {
                if (value is ObservableCollection<DataRowVM> collection)
                {
                    _selectedAllergens = collection;
                }
                else if (value != null)
                {
                    _selectedAllergens = new ObservableCollection<DataRowVM>(value.Cast<DataRowVM>());
                }
                else
                {
                    _selectedAllergens = new ObservableCollection<DataRowVM>();
                }

                OnPropertyChanged(nameof(SelectedAllergens));
            }
        }

        public ICommand AddItemCommand { get; }
        public ICommand CancelCommand { get; }

        public UpsertItemVM(INavigationService navigationService,IItemService itemService,IFoodCategoryService categoryService,
            IAllergenService allergenService,IItemPictureService itemPictureService)
        {
            _navigationService = navigationService;
            _itemService = itemService;
            _allergenService = allergenService;
            _itemPictureService = itemPictureService;

            AddItemCommand = new RelayCommand(UpsertItem_Execute, UpsertItem_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);

            FoodCategoryItems = new ObservableCollection<FoodCategory>(categoryService.GetAll());

            LoadItemPictures();
            LoadAllergens();
        }

        private void InitModifyMode()
        {
            ItemNameText = ItemToModify.Name;
            PriceText = ItemToModify.Price.ToString();
            PortionQuantityText = ItemToModify.PortionQuantity.ToString();
            TotalQuantityText = ItemToModify.TotalQuantity.ToString();

            SelectedFoodCategoryIndex = ItemToModify.FoodCategoryId - 1 ?? 0;

            _selectedItemPictures.Clear();
            _selectedAllergens.Clear();

            if (ItemToModify.Pictures != null)
            {
                foreach (var picture in ItemToModify.Pictures)
                {
                    _selectedItemPictures.Add(new DataRowVM(picture, CurrentColumnsItemPictures));
                }
            }
            OnPropertyChanged(nameof(SelectedItemPictures));

            if (ItemToModify.Allergens == null)
                return;

            foreach (var allergen in ItemToModify.Allergens)
            {
                _selectedAllergens.Add(new DataRowVM(allergen, CurrentColumnsAllergens));
            }
            OnPropertyChanged(nameof(SelectedAllergens));
        }

        public void LoadItemPictures()
        {
            _currentColumnsItemPictures = new Dictionary<string, GridColumnDefinition>()
            {
                ["Id"] = new GridColumnDefinition("Picture Id", "Id", typeof(int)),
                ["PicturePath"] = new GridColumnDefinition("Picture Path", "PicturePath", typeof(string))
            };

            CurrentDataItemPictures = new ObservableCollection<DataRowVM>();

            List<ItemPicture> itemPictures = _itemPictureService.GetAll();

            foreach (var itemPicture in itemPictures)
            {
                CurrentDataItemPictures.Add(new DataRowVM((itemPicture), CurrentColumnsItemPictures));
            }
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

        public async void UpsertItem_Execute()
        {
            if (ItemToModify == null)
            {
                try
                {
                    Item i = new Item()
                    {
                        Name = ItemNameText,
                        Price = decimal.Parse(PriceText),
                        PortionQuantity = float.Parse(PortionQuantityText),
                        TotalQuantity = float.Parse(TotalQuantityText),
                        FoodCategoryId = SelectedFoodCategoryIndex,
                        Allergens = new List<Allergen>(),
                        Pictures = new List<ItemPicture>()
                    };

                    foreach (DataRowVM allergen in _selectedAllergens)
                    {
                        i.Allergens.Add(allergen.GetOriginalData<Allergen>());
                    }

                    await _itemService.AddItemAsync(i);

                    foreach (DataRowVM itemPicture in _selectedItemPictures)
                    {
                        ItemPicture ip = itemPicture.GetOriginalData<ItemPicture>();
                        ip.ItemId = i.Id;
                        await _itemPictureService.UpdateItemPicture(ip);
                    }

                    MessageBox.Show("Item added in the database!","Success",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                catch (FormatException e)
                {
                    MessageBox.Show("You did not insert a valid number for price or quantity");
                }
            }
            else
            {
                ICollection<ItemPicture> modifiedItemPictures = new List<ItemPicture>();
                foreach(DataRowVM row in _selectedItemPictures)
                {
                    modifiedItemPictures.Add(row.GetOriginalData<ItemPicture>());
                }

                ICollection<Allergen> modifiedAllergens = new List<Allergen>();
                foreach (DataRowVM row in _selectedAllergens)
                {
                    modifiedAllergens.Add(row.GetOriginalData<Allergen>());
                }

                ItemToModify.Pictures = modifiedItemPictures;
                ItemToModify.Allergens = modifiedAllergens;
                ItemToModify.FoodCategoryId = SelectedFoodCategoryIndex;
                ItemToModify.Name = ItemNameText;
                ItemToModify.Price = decimal.Parse(PriceText);
                ItemToModify.PortionQuantity = float.Parse(PortionQuantityText);
                ItemToModify.TotalQuantity = float.Parse(TotalQuantityText);

                await _itemService.UpdateAsync(ItemToModify);

                MessageBox.Show("Item modified in the database!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public bool UpsertItem_CanExecute()
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
