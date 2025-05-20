using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public interface IItemPictureService
    {
        List<ItemPicture> GetAll();
        Task UpdateItemPicture(ItemPicture itemPicture);
        Task SaveChangesAsync();
        Task<IEnumerable<ItemPicture>> GetAllAsync();
    }
}
