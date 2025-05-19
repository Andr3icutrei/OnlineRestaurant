using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class Item : BaseEntity
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [Precision(10,2)]
        public decimal Price { get; set; }

        [Required]
        public float PortionQuantity { get; set; }

        [Required]
        public float TotalQuantity { get; set; }

        // foreign keys
        public int? FoodCategoryId { get; set; }
        public FoodCategory? FoodCategory { get; set; }

        // references
        public ICollection<Allergen> Allergens { get; set; } = new List<Allergen>();
        public ICollection<ItemPicture> Pictures { get; set; } = new List<ItemPicture>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<MenuItemConfiguration> MenuConfigurations { get; set; } = new List<MenuItemConfiguration>();
    }
}
