using OnlineRestaurant.Database.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Entities
{
    public class User : BaseEntity
    {
        [MaxLength(50)]
        public string FirstName {  get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Phone {  get; set; }

        [MaxLength(60)]
        public string Address { get; set; }

        [MaxLength(25),MinLength(5)]
        public string Password { get; set; }
        public UserType Type { get; set; }

        // references 
        public ICollection<Order> Orders { get; set; }
    }
}
