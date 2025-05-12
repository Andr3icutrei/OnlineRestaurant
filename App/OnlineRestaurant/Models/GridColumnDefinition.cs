using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.UI.Models
{
    public class GridColumnDefinition
    {
        public string Header { get; set; }
        public string PropertyPath { get; set; }
        public Type DataType { get; set; }
        public string StringFormat { get; set; }
        public bool IsReadOnly { get; set; }
        public Func<object, string> CustomFormatter { get; set; }  // Added for custom formatting

        public GridColumnDefinition(string header, string propertyPath, Type dataType = null,
                              string stringFormat = null, bool isReadOnly = true,
                              Func<object, string> customFormatter = null)
        {
            Header = header;
            PropertyPath = propertyPath;
            DataType = dataType ?? typeof(string);
            StringFormat = stringFormat;
            IsReadOnly = isReadOnly;
            CustomFormatter = customFormatter;
        }
    }

}
