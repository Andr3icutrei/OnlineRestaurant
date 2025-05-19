using Microsoft.EntityFrameworkCore;
using OnlineRestaurant.Database.Context;
using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using OnlineRestaurant.Database.Enums;

namespace OnlineRestaurant.Database.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public UserRepository(OnlineRestaurantDbContext context) : base(context)
        {
        }

        public async Task<bool> CanLoginUserAsync(string email, string password,UserType utype)
        {
            User? user = await GetRecords().FirstOrDefaultAsync(u => u.Email == email && u.Password == password && 
                u.Type == utype && u.DeletedAt == null);
            return user != null;
        }
    }
}
