using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public class ItemPictureService : IItemPictureService
    {
        private readonly IItemPictureRepository _itemPictureRepository;

        public ItemPictureService(IItemPictureRepository repo)
        {
            _itemPictureRepository = repo;
        }
        public List<ItemPicture> GetAll()
        {
            return _itemPictureRepository.GetAll();
        }

        public async Task<IEnumerable<ItemPicture>> GetAllAsync()
        {
            return await _itemPictureRepository.GetAllAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _itemPictureRepository.SaveChangesAsync();
        }

        public void UpdateItemPicture(ItemPicture itemPicture)
        {
            _itemPictureRepository.Update(itemPicture);
        }
    }
}
