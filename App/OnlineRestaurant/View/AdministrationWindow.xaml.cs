using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Shapes;

namespace OnlineRestaurant.UI.View
{
    /// <summary>
    /// Interaction logic for AdministrationWindow.xaml
    /// </summary>
    public partial class AdministrationWindow : Window
    {
        public AdministrationWindow()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var navigationService = serviceProvider.GetRequiredService<INavigationService>();

            DataContext = new AdministrationWindowVM(navigationService);
        }
    }
}
