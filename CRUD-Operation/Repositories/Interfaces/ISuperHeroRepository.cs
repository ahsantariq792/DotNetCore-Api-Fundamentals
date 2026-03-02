using CRUD_Operation.Entities;

namespace CRUD_Operation.Repositories.Interfaces
{
    public interface ISuperHeroRepository
    {
        Task<IEnumerable<SuperHero>> GetAllAsync();
        Task<SuperHero?> GetByIdAsync(int id);
        Task<SuperHero> AddAsync(SuperHero superHero);
        Task<SuperHero?> UpdateAsync(int id, SuperHero superHero);
        Task<bool> DeleteAsync(int id);
    }
}
