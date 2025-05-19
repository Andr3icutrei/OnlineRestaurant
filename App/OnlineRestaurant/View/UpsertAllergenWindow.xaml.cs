using Microsoft.Extensions.DependencyInjection;
using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddAllergenWindow.xaml
    /// </summary>
    public partial class UpsertAllergenWindow : Window, INavigationAware
    {
        private UpsertAllergenVM _vm;
        public UpsertAllergenWindow()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var vm = serviceProvider.GetRequiredService<UpsertAllergenVM>();

            _vm = vm;
            DataContext = _vm;
        }

        public void OnNavigatedTo(object parameter)
        {
            if(parameter is Allergen allergen)
            {
                _vm.AllergenToModify = allergen;
            }
        }
    }
}
