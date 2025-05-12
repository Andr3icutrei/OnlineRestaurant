using OnlineRestaurant.Database.Context;
using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(OnlineRestaurantDbContext context) : base(context) 
        {
            
        }
    }
}
