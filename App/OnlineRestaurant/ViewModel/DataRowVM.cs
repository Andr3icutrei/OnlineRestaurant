using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OnlineRestaurant.UI.Models;

namespace OnlineRestaurant.UI.ViewModel
{
    public class DataRowVM
    {
        public readonly object _data;        // The actual object (Customer, Product, etc.)
        private readonly string _type;        // Type identifier ("Customer", "Product")
        public readonly Dictionary<string, GridColumnDefinition> Columns; // Column definitions

        public DataRowVM(object data, Dictionary<string, GridColumnDefinition> columns)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _type = data.GetType().Name;
            Columns = columns;
        }

        // Main method to get property values - uses reflection
        public T GetValue<T>(string columnKey)
        {
            if (_data == null) return default;

            if (!Columns.TryGetValue(columnKey, out var columnDef))
                return default;

            if (string.IsNullOrEmpty(columnDef.PropertyPath))
                return default;

            var propertyInfo = _data.GetType().GetProperty(columnDef.PropertyPath);
            if (propertyInfo == null) return default;

            var value = propertyInfo.GetValue(_data);

            if (value == null) return default;

            // Handle enum types
            if (propertyInfo.PropertyType.IsEnum && typeof(T) == typeof(string))
            {
                // Use custom formatter if available
                if (columnDef.CustomFormatter != null)
                {
                    return (T)(object)columnDef.CustomFormatter(value);
                }

                // Convert enum to string with description attribute if available
                return (T)(object)GetEnumDescription(value);
            }

            // Regular type conversion
            try
            {
                if (value is T typedValue)
                    return typedValue;

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        private string GetEnumDescription(object enumValue)
        {
            if (enumValue == null) return string.Empty;

            var type = enumValue.GetType();
            var name = Enum.GetName(type, enumValue);

            if (string.IsNullOrEmpty(name)) return enumValue.ToString();

            // Check for Description attribute
            var field = type.GetField(name);
            var descriptionAttribute = field?.GetCustomAttribute<DescriptionAttribute>();

            if (descriptionAttribute != null)
                return descriptionAttribute.Description;

            // Convert camelCase/PascalCase to "Sentence Case"
            return ConvertToSentenceCase(name);
        }

        private string ConvertToSentenceCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // Add space before uppercase letters
            var result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (i > 0 && char.IsUpper(input[i]) && !char.IsUpper(input[i - 1]))
                {
                    result.Append(' ');
                }
                result.Append(input[i]);
            }

            return result.ToString();
        }

        // Overloaded version that returns object
        public object GetValue(string propertyName)
        {
            return GetValue<object>(propertyName);
        }

        // Indexer for easier access
        public object this[string columnName]
        {
            get => GetValue(columnName);
        }

        // Get the original data object (for when you need full access)
        public T GetOriginalData<T>() where T : class
        {
            return _data as T;
        }

        // Helper property to get the type
        public string DataType => _type;
    }
}
