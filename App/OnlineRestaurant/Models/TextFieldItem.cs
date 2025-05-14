using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.UI.Models
{
    public class TextFieldItem
    {
        private string _value;
        private string _labelText;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }

        public string LabelText
        {
            get { return _labelText; }
            set
            {
                _labelText = value;
            }
        }
    }
}
