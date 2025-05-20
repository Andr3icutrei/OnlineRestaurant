using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> CanLoginUserAsync(string email,string password,UserType uytpe);
    }
}
