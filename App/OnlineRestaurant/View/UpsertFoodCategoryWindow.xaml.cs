using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AddFoodCategoryWindow.xaml
    /// </summary>
    public partial class UpsertFoodCategoryWindow : Window, INavigationAware
    {
        private UpsertFoodCategoryVM _vm;
        public UpsertFoodCategoryWindow()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;

            _vm = serviceProvider.GetRequiredService<UpsertFoodCategoryVM>();
            DataContext = _vm;
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is FoodCategory category)
            { 
                _vm.FoodCategoryToModify = category;
            }
        }
    }
}
