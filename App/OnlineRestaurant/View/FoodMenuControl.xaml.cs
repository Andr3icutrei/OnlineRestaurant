using Microsoft.Extensions.DependencyInjection;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineRestaurant.UI.View
{
    /// <summary>
    /// Interaction logic for FoodMenuUserControl.xaml
    /// </summary>
    public partial class FoodMenuControl : UserControl
    {
        private FoodMenuControlVM _viewModel;

        public FoodMenuControl()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var viewModel = serviceProvider.GetRequiredService<FoodMenuControlVM>();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        public async Task OnNavigatedTo(object parameter)
        {
            if (parameter is User currentUser)
            { 
                await _viewModel.SetUser(currentUser); 
            }
        }

        // Search event handlers
        private void ItemSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SearchItems();
            }
        }

        private void MenuSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SearchMenus();
            }
        }

        private void SearchCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            // When any NOT checkbox changes, trigger appropriate search
            if (sender is CheckBox checkBox)
            {
                if (checkBox.Content.ToString() == "NOT")
                {
                    // Determine which search to trigger based on the binding
                    if (checkBox.IsChecked.HasValue && checkBox.DataContext == _viewModel)
                    {
                        if (_viewModel.ItemNameSearchText != null || _viewModel.ItemAllergenSearchText != null)
                        {
                            _viewModel.SearchItems();
                        }
                        else if (_viewModel.MenuSearchText != null)
                        {
                            _viewModel.SearchMenus();
                        }
                    }
                }
            }
        }

        private void SearchItems_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SearchItems();
            }
        }

        private void SearchMenus_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SearchMenus();
            }
        }


    }
}
