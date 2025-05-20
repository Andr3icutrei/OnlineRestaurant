using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.UI.Models
{
    public class OrderDisplayDataGrid
    {
        public int Id { get; }
        public decimal Price { get; }
        public OrderState State { get; }
        public string ItemDescription { get; }
        public string MenuDescription { get; }

        public OrderDisplayDataGrid(int id,decimal price,OrderState state, string itemDescription, string menuDescription)
        {
            Id = id;
            Price = price;
            State = state;
            ItemDescription = itemDescription;
            MenuDescription = menuDescription;
        }
    }
}
