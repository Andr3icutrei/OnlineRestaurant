using OnlineRestaurant.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace OnlineRestaurant.UI.Converters
{
    public class DynamicPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataRowVM row && parameter is string columnKey)
            {
                return row.GetValue(columnKey);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Implement if you need two-way binding
            return DependencyProperty.UnsetValue;
        }
    }
}
