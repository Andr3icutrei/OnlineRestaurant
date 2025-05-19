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
    public class UpsertAllergenVM : INotifyPropertyChanged
    {
        private Allergen? _allergenToModify;
        public Allergen? AllergenToModify
        {
            get => _allergenToModify;
            set
            {
                _allergenToModify = value;
                InitModifyAllergenMode();
            }
        }

        private readonly INavigationService _navigationService;
        private readonly IAllergenService _allergenService;

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

        public ICommand AllergenCommand { get; }
        public ICommand CancelCommand { get; }

        public UpsertAllergenVM(INavigationService navigationService, IAllergenService service)
        {
            AllergenNameText = string.Empty;

            _navigationService = navigationService;
            _allergenService = service;

            AllergenCommand = new AsyncRelayCommand(UpsertAllergen_Execute, UpsertAllergen_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);
        }

        public void InitModifyAllergenMode()
        {
            if (AllergenToModify != null)
            { 
                AllergenNameText = AllergenToModify.Type;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task UpsertAllergen_Execute()
        {
            if (AllergenToModify == null)
            {
                Allergen allergen = new Allergen() { Type = AllergenNameText };
                if (await _allergenService.IsTypeUniqueAsync(allergen))
                {
                    await _allergenService.AddAllergenAsync(allergen);
                    ClearTextBoxes();
                    MessageBox.Show("Allergen added to the database!","Succes",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Allergen already exists!","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            else
            {
                if (AllergenToModify.Type != AllergenNameText)
                {
                    AllergenToModify.Type = AllergenNameText; 
                    await _allergenService.Update(AllergenToModify);
                    MessageBox.Show("Allergen modified in the database!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public bool UpsertAllergen_CanExecute()
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
