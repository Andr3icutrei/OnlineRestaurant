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
        public int Id { get; set; }
        public decimal Price { get; set; }
        public OrderState State { get; set; }
        public string ItemDescription { get; set; }
        public string MenuDescription { get; set; }
        public string CreatedAt { get; set; }
    }
}
