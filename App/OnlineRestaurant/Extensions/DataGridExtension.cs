using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;

namespace OnlineRestaurant.UI.Extensions
{
    public static class DataGridExtension
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(DataGridExtension),
                new PropertyMetadata(null, OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                // Remove the previous handler if any
                if (e.OldValue != null)
                {
                    dataGrid.SelectionChanged -= OnDataGridSelectionChanged;
                }

                // Add new handler if we have a new value
                if (e.NewValue != null)
                {
                    // Make sure the DataGrid can select multiple items
                    if (dataGrid.SelectionMode == DataGridSelectionMode.Single)
                    {
                        dataGrid.SelectionMode = DataGridSelectionMode.Extended;
                    }

                    // Hook up the SelectionChanged event
                    dataGrid.SelectionChanged += OnDataGridSelectionChanged;

                    // Initialize the DataGrid selection based on the bound collection
                    UpdateDataGridSelection(dataGrid, e.NewValue as IList);
                }
            }
        }

        private static void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                // Get the bound collection
                IList boundList = GetSelectedItems(dataGrid);
                if (boundList == null) return;

                // Add newly selected items
                foreach (var item in e.AddedItems)
                {
                    if (!boundList.Contains(item))
                    {
                        boundList.Add(item);
                    }
                }

                // Remove deselected items
                foreach (var item in e.RemovedItems)
                {
                    boundList.Remove(item);
                }

                // Debugging output
                Debug.Print($"Selection changed. Total selected items: {boundList.Count}");
            }
        }

        // Method to update DataGrid selection from the bound collection
        private static void UpdateDataGridSelection(DataGrid dataGrid, IList collection)
        {
            if (collection == null || dataGrid == null) return;

            // Clear current selection
            dataGrid.SelectedItems.Clear();

            // Add all items from the bound collection to the selection
            foreach (var item in collection)
            {
                dataGrid.SelectedItems.Add(item);
            }
        }
    }
}
