using OnlineRestaurant.Database.Context;
using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public class ItemPictureRepository : BaseRepository<ItemPicture>, IItemPictureRepository
    {
        public ItemPictureRepository(OnlineRestaurantDbContext context) : base(context)
        {
            
        }

        public List<ItemPicture> GetAll()
        {
            return GetRecords().ToList();
        }
    }
}
