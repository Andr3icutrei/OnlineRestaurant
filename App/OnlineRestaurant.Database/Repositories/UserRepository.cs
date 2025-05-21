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
using Microsoft.Data.SqlClient;
using System.Data;

namespace OnlineRestaurant.Database.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        public UserRepository(OnlineRestaurantDbContext context) : base(context)
        {
        }

        public async Task<User?> CanLoginUserAsync(string email, string password,UserType utype)
        {
            return await GetRecords().FirstOrDefaultAsync(u => u.Email == email && u.Password == password && 
                u.Type == utype && u.DeletedAt == null);
        }

        public async Task<User?> CanLoginUserProcedureAsync(string email, string password, UserType utype)
        {
            var parameters = new[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", password),
                new SqlParameter("@UserType", (int)utype)
            };

            using var command = _databaseContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = "CanLoginUser";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@Email", email));
            command.Parameters.Add(new SqlParameter("@Password", password));
            command.Parameters.Add(new SqlParameter("@UserType", (int)utype));

            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    Type = (UserType)reader.GetInt32(reader.GetOrdinal("Type")),
                };
            }

            return null;
        }
    }
}
