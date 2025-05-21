using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineRestaurant.Database.Context;
using OnlineRestaurant.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Repositories
{
    public class AllergenRepository : BaseRepository<Allergen>, IAllergenRepository
    {
        public AllergenRepository(OnlineRestaurantDbContext context) : base(context)
        {
            
        }

        public async Task<bool> IsTypeUniqueAsync(Allergen allergen)
        {
            return !(await GetRecords().AnyAsync(a => a.Type == allergen.Type));
        }

        public async Task<bool> IsTypeUniqueStoredAsync(Allergen allergen)
        {
            var isUniqueParameter = new SqlParameter
            {
                ParameterName = "@IsUnique",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Output
            };

            var allergenTypeParameter = new SqlParameter
            {
                ParameterName = "@AllergenType",
                SqlDbType = SqlDbType.NVarChar,
                Size = 255, // Adjust size based on your database column definition
                Value = allergen.Type
            };

            // Execute the stored procedure
            await _databaseContext.Database.ExecuteSqlRawAsync(
                "EXEC IsAllergenTypeUnique @AllergenType, @IsUnique OUTPUT",
                allergenTypeParameter, isUniqueParameter);

            // Get the output parameter value and negate it as per original method
            return isUniqueParameter.Value != DBNull.Value &&
                   Convert.ToBoolean(isUniqueParameter.Value);
        }
    }
}
