using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using OnlineRestaurant.UI.Models;

namespace OnlineRestaurant.UI.Services
{
    public class DynamicColumnsBehavior : Behavior<DataGrid>
    {
        public static readonly DependencyProperty ColumnsSourceProperty =
            DependencyProperty.Register(
                "ColumnsSource",
                typeof(IEnumerable<KeyValuePair<string, GridColumnDefinition>>),
                typeof(DynamicColumnsBehavior),
                new PropertyMetadata(null, OnColumnsSourceChanged));

        public IEnumerable<KeyValuePair<string, GridColumnDefinition>> ColumnsSource
        {
            get => (IEnumerable<KeyValuePair<string, GridColumnDefinition>>)GetValue(ColumnsSourceProperty);
            set => SetValue(ColumnsSourceProperty, value);
        }

        private static void OnColumnsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (DynamicColumnsBehavior)d;
            behavior.UpdateColumns();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            UpdateColumns();
        }

        private void UpdateColumns()
        {
            if (AssociatedObject == null || ColumnsSource == null)
                return;

            AssociatedObject.Columns.Clear();

            foreach (var columnPair in ColumnsSource)
            {
                var columnDef = columnPair.Value;
                var columnKey = columnPair.Key;

                // Create binding using converter
                var binding = new Binding(".")
                {
                    Converter = AssociatedObject.FindResource("DynamicPropertyConverter") as IValueConverter,
                    ConverterParameter = columnKey
                };

                // Apply string format if provided
                if (!string.IsNullOrEmpty(columnDef.StringFormat))
                {
                    binding.StringFormat = columnDef.StringFormat;
                }

                // Create appropriate column type
                DataGridColumn gridColumn;

                if (columnDef.DataType == typeof(bool))
                {
                    gridColumn = new DataGridCheckBoxColumn
                    {
                        Header = columnDef.Header,
                        Binding = binding
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

                AssociatedObject.Columns.Add(gridColumn);
            }
        }
    }
}
