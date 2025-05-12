using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineRestaurant.UI.Services
{
    public class NavigationService : INavigationService
    {

        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            window.Show();

            // Close the current window
            if (Application.Current.MainWindow != null && Application.Current.MainWindow.IsLoaded)
            {
                Application.Current.MainWindow.Close();
            }

            // Set the new window as the main window
            Application.Current.MainWindow = window;
        }
    }   
}
