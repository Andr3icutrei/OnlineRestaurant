using Microsoft.Extensions.DependencyInjection;
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

namespace OnlineRestaurant.UI.View
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window, INavigationAware
    {
        private UserWindowVM _viewmodel;
        public UserWindow()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var viewmodel = serviceProvider.GetRequiredService<UserWindowVM>();
            _viewmodel = viewmodel;
            DataContext = _viewmodel;
        }

        public void OnNavigatedTo(object parameter)
        {
            if(parameter is User user)
            {
                _viewmodel.ConfigureInit(user);
            }
        }
    }
}
