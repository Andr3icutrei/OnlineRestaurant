using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public interface IUserService
    {
        Task AddUserAsync(User user);
        Task<bool> CanLoginUserAsync(string Password, string UserName,UserType utype);
    }
}
