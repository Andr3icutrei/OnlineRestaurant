using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Models;
using OnlineRestaurant.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace OnlineRestaurant.UI.Converters
{
    public class ItemQuantityConverter : IValueConverter
    {
        private static Dictionary<int, int> _itemQuantities = new Dictionary<int, int>();

        public static void SetQuantity(int itemId, int quantity)
        {
            _itemQuantities[itemId] = quantity;
        }

        public static int GetQuantity(int itemId)
        {
            return _itemQuantities.ContainsKey(itemId) ? _itemQuantities[itemId] : 0;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OrderDisplay item)
            {
                return GetQuantity(item.Id);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
