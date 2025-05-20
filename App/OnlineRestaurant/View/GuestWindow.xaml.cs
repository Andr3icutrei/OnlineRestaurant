using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Shapes;

namespace OnlineRestaurant.UI.View
{
    /// <summary>
    /// Interaction logic for GuestWindow.xaml
    /// </summary>
    public partial class GuestWindow : Window
    {
        private GuestWindowVM _viewmodel;

        public GuestWindow()
        {
            InitializeComponent();
            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var guestVM = serviceProvider.GetRequiredService<GuestWindowVM>();
            _viewmodel = guestVM;
            DataContext = _viewmodel;
        }

        // Event handlers for search functionality
        private void ItemSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewmodel.SearchItems();
        }

        private void MenuSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewmodel.SearchMenus();
        }

        private void SearchCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            // Trigger search when NOT checkbox state changes
            if (sender is CheckBox checkBox)
            {
                // Determine which search to trigger based on the checkbox's binding
                if (checkBox.IsChecked.HasValue)
                {
                    // Check data binding to determine which search to trigger
                    if (checkBox.GetBindingExpression(CheckBox.IsCheckedProperty)?.ResolvedSourcePropertyName == "IsItemNameExcluded" ||
                        checkBox.GetBindingExpression(CheckBox.IsCheckedProperty)?.ResolvedSourcePropertyName == "IsItemAllergenExcluded")
                    {
                        _viewmodel.SearchItems();
                    }
                    else if (checkBox.GetBindingExpression(CheckBox.IsCheckedProperty)?.ResolvedSourcePropertyName == "IsMenuNameExcluded")
                    {
                        _viewmodel.SearchMenus();
                    }
                }
            }
        }

        // Button click handlers
        private void SearchItems_Button_Click(object sender, RoutedEventArgs e)
        {
            _viewmodel.SearchItems();
        }

        private void SearchMenus_Button_Click(object sender, RoutedEventArgs e)
        {
            _viewmodel.SearchMenus();
        }
    }
}
