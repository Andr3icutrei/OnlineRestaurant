using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class ItemPicture : BaseEntity
    {
        [Required]
        public string PicturePath { get; set; }
        
        //foreign keys
        public int? ItemId { get; set; }
        public Item? Item { get; set; }
    }
}
