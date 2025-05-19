using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using OnlineRestaurant.UI.ViewModel;
using OnlineRestaurant.Database.Entities;
using System.Diagnostics.Metrics;

namespace OnlineRestaurant.UI.Extensions
{
    public static class DataGridExtension
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(DataGridExtension),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsChanged));

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
                Debug.Print($"OnSelectedItemsChanged called. Old value: {e.OldValue}, New value: {e.NewValue}");

                dataGrid.SelectionChanged -= OnDataGridSelectionChanged;

                if (e.NewValue != null)
                {
                    if (dataGrid.SelectionMode == DataGridSelectionMode.Single)
                    {
                        dataGrid.SelectionMode = DataGridSelectionMode.Extended;
                        Debug.Print("Changed DataGrid SelectionMode to Extended");
                    }

                    dataGrid.SelectionChanged += OnDataGridSelectionChanged;
                    Debug.Print("Hooked up SelectionChanged event handler");

                    dataGrid.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        UpdateDataGridSelection(dataGrid, e.NewValue as IList);
                    }), DispatcherPriority.Loaded);
                }
            }
        }

        private static void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                IList boundList = GetSelectedItems(dataGrid);
                if (boundList == null) return;

                Debug.Print($"SelectionChanged event fired. Added: {e.AddedItems.Count}, Removed: {e.RemovedItems.Count}");

                dataGrid.SelectionChanged -= OnDataGridSelectionChanged;

                try
                {
                    foreach (var item in e.AddedItems)
                    {
                        if (!boundList.Contains(item))
                        {
                            boundList.Add(item);
                            Debug.Print($"Added item to bound list: {item}");
                        }
                    }

                    foreach (var item in e.RemovedItems)
                    {
                        boundList.Remove(item);
                        Debug.Print($"Removed item from bound list: {item}");
                    }
                }
                finally
                {
                    dataGrid.SelectionChanged += OnDataGridSelectionChanged;
                }

                Debug.Print($"Selection changed. Total selected items: {boundList.Count}");
            }
        }

        private static void UpdateDataGridSelection(DataGrid dataGrid, IList collection)
        {
            if (collection == null || dataGrid == null || dataGrid.ItemsSource == null) return;

            dataGrid.SelectionChanged -= OnDataGridSelectionChanged;
            try
            {
                dataGrid.SelectedItems.Clear();
                List<DataRowVM> itemsSource = dataGrid.ItemsSource.Cast<DataRowVM>().ToList();

                int matchCount = 0;
                foreach (var selectedItem in collection)
                {
                    if (selectedItem is DataRowVM selectedDataRowVM)
                    {
                        var selectedEntity = selectedDataRowVM.GetOriginalData<BaseEntity>();

                        if (selectedEntity != null)
                        {
                            int selectedId = selectedEntity.Id;

                            DataRowVM matchingItem = itemsSource.FirstOrDefault(item => {
                                var entity = item.GetOriginalData<BaseEntity>();
                                return entity != null && entity.Id == selectedId;
                            });

                            if (matchingItem != null)
                            {
                                dataGrid.SelectedItems.Add(matchingItem);
                                matchCount++;
                            }
                        }
                    }
                }

                // One line of debug information showing the key results
                Debug.Print($"Selection result: {matchCount}/{collection.Count} items matched, final selection count: {dataGrid.SelectedItems.Count}");
            }
            finally
            {
                dataGrid.SelectionChanged += OnDataGridSelectionChanged;
            }
        }
    }
}
