using CRUD_Operation.Entities;
using CRUD_Operation.Repositories.Interfaces;
using CRUD_Operation.Services.Interfaces;

namespace CRUD_Operation.Services.Implementations
{
    public class SuperHeroService : ISuperHeroService
    {
        private readonly ISuperHeroRepository _repository;

        public SuperHeroService(ISuperHeroRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SuperHero>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<SuperHero?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<SuperHero> CreateAsync(SuperHero superHero)
        {
            return await _repository.AddAsync(superHero);
        }

        public async Task<SuperHero?> UpdateAsync(int id, SuperHero superHero)
        {
            return await _repository.UpdateAsync(id, superHero);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
