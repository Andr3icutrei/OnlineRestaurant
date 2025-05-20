using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.UI.Models
{
    public class OrderDisplay : INotifyPropertyChanged
    {
        public bool IsItem {  get; set; }
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
                UpdateTotalPrice();
            }
        }

        private int _portionQuantity;
        public int PortionQuantity
        {
            get => _portionQuantity;
            set
            {
                _portionQuantity = value;
                OnPropertyChanged();
            }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                _unitPrice = value;
                OnPropertyChanged();
                UpdateTotalPrice();
            }
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        private void UpdateTotalPrice()
        {
            TotalPrice = UnitPrice * Quantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public float TotalQuantity { get; private set; }
    }
}
