using Microsoft.Extensions.DependencyInjection;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineRestaurant.UI.View
{
    /// <summary>
    /// Interaction logic for OrdersUserControl.xaml
    /// </summary>
    public partial class OrdersControl : UserControl
    {
        private OrdersControlVM _viewmodel;
        public OrdersControl(User user)
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var ordersControlVM = serviceProvider.GetRequiredService<OrdersControlVM>();

            _viewmodel = ordersControlVM;
            _viewmodel.PropertyChanged += ViewModel_PropertyChanged;
            _viewmodel.CurrentUser = user;

            DataContext = _viewmodel;
            UpdateGridColumns(DynamicGrid, _viewmodel.GridColumns);
        }
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewmodel.GridColumns))
            {
                UpdateGridColumns(DynamicGrid, _viewmodel.GridColumns);
            }
        }

        public void UpdateGridColumns(DataGrid grid, IEnumerable<KeyValuePair<string, GridColumnDefinition>> gridColumns)
        {
            grid.Columns.Clear();

            foreach (var columnPair in gridColumns)
            {
                var columnDef = columnPair.Value;
                var columnKey = columnPair.Key;

                var binding = new Binding(".")
                {
                    Converter = FindResource("DynamicPropertyConverter") as IValueConverter,
                    ConverterParameter = columnKey
                };

                if (!string.IsNullOrEmpty(columnDef.StringFormat))
                {
                    binding.StringFormat = columnDef.StringFormat;
                }

                DataGridColumn gridColumn;

                if (columnDef.DataType == typeof(bool))
                {
                    gridColumn = new DataGridCheckBoxColumn
                    {
                        Header = columnDef.Header,
                        Binding = binding
                    };
                }
                else if (columnDef.DataType == typeof(DateTime))
                {
                    gridColumn = new DataGridTextColumn
                    {
                        Header = columnDef.Header,
                        Binding = binding,
                        SortMemberPath = columnKey
                    };
                }
                else
                {
                    gridColumn = new DataGridTextColumn
                    {
                        Header = columnDef.Header,
                        Binding = binding,
                        SortMemberPath = columnKey
                    };
                }

                grid.Columns.Add(gridColumn);
            }
        }
    }
}
