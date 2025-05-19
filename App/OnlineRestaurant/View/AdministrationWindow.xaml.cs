using Microsoft.Extensions.DependencyInjection;
using OnlineRestaurant.Core.Services;
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
using System.Windows.Shapes;

namespace OnlineRestaurant.UI.View
{
    /// <summary>
    /// Interaction logic for AdministrationWindow.xaml
    /// </summary>
    public partial class AdministrationWindow : Window, INavigationAware
    {
        private readonly AdministrationWindowVM _viewModel;
        public AdministrationWindow()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;
            var administrationWindowVM = serviceProvider.GetRequiredService<AdministrationWindowVM>();

            _viewModel = administrationWindowVM;
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            DataContext = _viewModel;

            UpdateGridColumns(DynamicGrid, _viewModel.GridColumns);
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.GridColumns))
            {
                 UpdateGridColumns(DynamicGrid, _viewModel.GridColumns);
            }
        }

        public void UpdateGridColumns(DataGrid grid, IEnumerable<KeyValuePair<string, GridColumnDefinition>> gridColumns)
        {
            grid.Columns.Clear();

            foreach (var columnPair in gridColumns)
            {
                var columnDef = columnPair.Value;
                var columnKey = columnPair.Key;

                // Create a binding that uses our converter
                var binding = new Binding(".")
                {
                    Converter = FindResource("DynamicPropertyConverter") as IValueConverter,
                    ConverterParameter = columnKey
                };

                // Apply string format if provided
                if (!string.IsNullOrEmpty(columnDef.StringFormat))
                {
                    binding.StringFormat = columnDef.StringFormat;
                }

                // Create the appropriate column type based on data type
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

        public void OnNavigatedTo(object parameter)
        {
            if(parameter is string adminName)
            {
                _viewModel.AdminNameText = adminName;
            }
        }
    }
}
