using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OnlineRestaurant.UI.ViewModel
{
    public class AddAllergenVM : INotifyPropertyChanged
    {
        private string _allergenNameText;
        public string AllergenNameText
        {
            get => _allergenNameText;
            set
            {
                _allergenNameText = value;
                OnPropertyChanged(nameof(AllergenNameText));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly INavigationService _navigationService;
        private readonly IAllergenService _allergenService;

        public ICommand AddAllergenCommand { get; }
        public ICommand CancelCommand { get; }

        public AddAllergenVM(INavigationService navigationService, IAllergenService service)
        {
            AllergenNameText = string.Empty;

            _navigationService = navigationService;
            _allergenService = service;

            AddAllergenCommand = new RelayCommand(AddAllergen_Execute, AddAllergen_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void AddAllergen_Execute()
        {
            Allergen allergen = new Allergen() { Type = AllergenNameText };
            if(await _allergenService.IsTypeUniqueAsync(allergen))
            {
                await _allergenService.AddAllergenAsync(allergen);
                ClearTextBoxes();
                MessageBox.Show("Allergen added to the database!");
            }
            else
            {
                MessageBox.Show("Allergen already exists!");
            }
        }

        public bool AddAllergen_CanExecute()
        {
            return AllergenNameText != string.Empty;
        }

        public void Cancel_Execute()
        {
            _navigationService.NavigateTo<AdministrationWindow>();
        }

        private void ClearTextBoxes()
        {
            AllergenNameText = string.Empty;
        }
    }
}
