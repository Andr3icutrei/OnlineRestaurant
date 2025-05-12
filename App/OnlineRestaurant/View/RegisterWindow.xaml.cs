using OnlineRestaurant.Core.Services;
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
using System.Windows.Shapes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineRestaurant.UI.View
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            // Get services from App's service provider
            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var userService = serviceProvider.GetRequiredService<IUserService>();
            var navigationService = serviceProvider.GetRequiredService<INavigationService>();

            // Create the ViewModel with dependencies
            DataContext = new RegisterWindowVM(userService, navigationService);
        }
    }
}
