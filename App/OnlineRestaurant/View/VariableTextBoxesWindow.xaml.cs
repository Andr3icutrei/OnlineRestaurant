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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineRestaurant.UI.View
{
    /// <summary>
    /// Interaction logic for VariableTextBoxesWindow.xaml
    /// </summary>
    public partial class VariableTextBoxesWindow : Window
    {
        public VariableTextBoxesWindow(List<Item> selectedItems)
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var userService = serviceProvider.GetRequiredService<IUserService>();
            var navigationService = serviceProvider.GetRequiredService<INavigationService>();

            DataContext = new VariableTextBoxesVM(selectedItems, navigationService);
        }
    }
}
