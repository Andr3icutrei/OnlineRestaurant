using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OnlineRestaurant.UI.Services
{
    public interface INavigationService
    {
        void NavigateTo<T>(object parameter = null, bool close = true) where T : Window;
    }

    public interface INavigationServiceAsync
    {
        Task NavigateToAsync<T>(object parameter = null,bool close = true) where T : Window;
    }
}
