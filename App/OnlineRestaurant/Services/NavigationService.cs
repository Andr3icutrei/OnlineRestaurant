using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OnlineRestaurant.UI.Services
{
    public class NavigationService : INavigationService
    {

        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<T>(object parameter = null, bool close = true) where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>(); 

            if (parameter != null && window is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(parameter);
            }

            window.Show();

            if (close && Application.Current.MainWindow != null && Application.Current.MainWindow.IsLoaded)
            {
                Application.Current.MainWindow.Close();
            }

            Application.Current.MainWindow = window;
        }
    }

    public class NavigationServiceAsync : INavigationServiceAsync
    {

        private readonly IServiceProvider _serviceProvider;

        public NavigationServiceAsync(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToAsync<T>(object parameter = null, bool close = true) where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();

            if (parameter != null && window is INavigationAwareAsync navigationAware)
            {
                await navigationAware.OnNavigatedToAsync(parameter);
            }

            window.Show();

            if (close && Application.Current.MainWindow != null && Application.Current.MainWindow.IsLoaded)
            {
                Application.Current.MainWindow.Close();
            }

            Application.Current.MainWindow = window;
        }
    }
}
