using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineRestaurant.UI.View
{
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var navigationService = serviceProvider.GetRequiredService<INavigationService>();

            DataContext = new StartupWindowVM(navigationService);
        }
    }
}