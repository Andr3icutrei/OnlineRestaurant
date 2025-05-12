using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync(bool includeDeletedEntities = false);
        List<T> GetAll(bool includeDeletedEntities = false);
        Task<T?> GetFirstOrDefaultAsync(int primaryKey, bool includeDeletedEntities = false);
        void Insert(params T[] records);
        void Update(params T[] records);
        void SoftDelete(params T[] records);
        Task SaveChangesAsync();
    }
}
