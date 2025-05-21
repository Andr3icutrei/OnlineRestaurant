using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.UI.Services
{
    public interface INavigationAware
    {
        void OnNavigatedTo(object parameter);
    }

    public interface INavigationAwareAsync
    {
        Task OnNavigatedToAsync(object parameter);
    }
}
