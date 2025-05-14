using OnlineRestaurant.Database.Entities;
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
    public class VariableTextBoxesVM : ICloseable
    {
        public Action RequestClose { get; set; }
        public ICommand ConfirmQuantitiesCommand { get; }
        public ICommand CancelCommand { get; }
        public ObservableCollection<TextFieldItem> TextFieldItems { get; set; }
        private readonly INavigationService _navigationService;
        public bool WasConfirmed { get; set; }

        public VariableTextBoxesVM(List<Item> selectedItems,INavigationService navigationService)
        {
            _navigationService = navigationService;
            ConfirmQuantitiesCommand = new RelayCommand(ConfirmQuantities_Execute, ConfirmQuantities_CanExecute);
            CancelCommand = new RelayCommand(Cancel_Execute);

            TextFieldItems = new ObservableCollection<TextFieldItem>();

            InitializeLabels(selectedItems);
        }

        public void ConfirmQuantities_Execute()
        {
            WasConfirmed = true;
            RequestClose?.Invoke();
        }

        public bool ConfirmQuantities_CanExecute()
        {
            for (int i = 0; i < TextFieldItems.Count; i++)
            {
                if(TextFieldItems[i].Value == string.Empty)
                    return false;
            }
            return true;
        }

        public void Cancel_Execute()
        {
            WasConfirmed = false;
            RequestClose?.Invoke();
        }

        private void InitializeLabels(List<Item> selectedItems)
        {
            for (int i = 0; i < selectedItems.Count; i++)
            {
                TextFieldItems.Add(new TextFieldItem { LabelText = selectedItems[i].Name, Value = string.Empty });
            }
        }
    }
}
