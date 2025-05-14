using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineRestaurant.UI.Services
{
    public interface IWindowService
    {
        void ShowWindow<T>(object dataContext = null, object owner = null) where T : Window, new();
        bool? ShowDialog<T>(object dataContext = null, object owner = null) where T : Window, new();
        void CloseWindow(Window window);
        void ShowVariableTextBoxesWindow(
            List<Item> selectedItems,
            object owner,
            Action<VariableTextBoxesVM, bool> resultCallback);
    }
}
