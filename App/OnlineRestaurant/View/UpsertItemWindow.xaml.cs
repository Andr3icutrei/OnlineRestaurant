using Microsoft.Extensions.DependencyInjection;
using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Models;
using OnlineRestaurant.UI.Services;
using OnlineRestaurant.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class UpsertItemWindow : Window, INavigationAware
    {
        private UpsertItemVM _viewModel;
        public UpsertItemWindow()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).HostInstance.Services;

            _viewModel = serviceProvider.GetRequiredService<UpsertItemVM>();
            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;

            UpdateGridColumns(DynamicGridAllergens, _viewModel.GridColumnsAllergens);
            UpdateGridColumns(DynamicGridItemPictures, _viewModel.GridColumnsItemPictures);
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.GridColumnsAllergens))
            {
                UpdateGridColumns(DynamicGridAllergens, _viewModel.GridColumnsAllergens);
            }
            else if (e.PropertyName == nameof(_viewModel.GridColumnsItemPictures))
            {
                UpdateGridColumns(DynamicGridItemPictures, _viewModel.GridColumnsItemPictures);
            }
        }

        private void UpdateGridColumns(DataGrid grid,IEnumerable<KeyValuePair<string,Models.GridColumnDefinition>> gridColumns)
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

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!text.All(char.IsDigit))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+[\\.]?[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void OnNavigatedTo(object parameter)
        {
            if(parameter is Item item)
            {
                _viewModel.ItemToModify = item;
            }
        } 
    }
}
