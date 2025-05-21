using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Enums;
using OnlineRestaurant.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task AddUserAsync(User entity)
        {
            _repository.Insert(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<User?> CanLoginUserAsync(string password, string username, UserType utype)
        {
            return await _repository.CanLoginUserProcedureAsync(password, username, utype);
        }
    }
}
