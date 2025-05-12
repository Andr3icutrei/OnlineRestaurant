using Microsoft.Extensions.DependencyInjection;
using OnlineRestaurant.Core.Services;
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
    public partial class AddAllergenWindow : Window
    {
        public AddAllergenWindow()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var navigationService = serviceProvider.GetRequiredService<INavigationService>();
            var allergenService = serviceProvider.GetRequiredService<IAllergenService>();

            DataContext = new AddAllergenVM(navigationService, allergenService);
        }
    }
}
