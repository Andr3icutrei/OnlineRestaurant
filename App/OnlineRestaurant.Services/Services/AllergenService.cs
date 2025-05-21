using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Core.Services
{
    public class AllergenService : IAllergenService
    {
        private readonly IAllergenRepository _repository;

        public AllergenService(IAllergenRepository repository)
        {
            _repository = repository;
        }
        public async Task AddAllergenAsync(Allergen allergen)
        {
            _repository.Insert(allergen);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteByIdAsync(id);
        }

        public IEnumerable<Allergen> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<IEnumerable<Allergen>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<bool> IsTypeUniqueAsync(Allergen allergen)
        {
            return await _repository.IsTypeUniqueStoredAsync(allergen);
        }

        public async Task Update(Allergen allergen)
        {
            await _repository.Update(allergen);
        }
    }
}
