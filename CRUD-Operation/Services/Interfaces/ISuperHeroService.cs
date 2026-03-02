using CRUD_Operation.Entities;

namespace CRUD_Operation.Services.Interfaces
{
    public interface ISuperHeroService
    {
        Task<IEnumerable<SuperHero>> GetAllAsync();
        Task<SuperHero?> GetByIdAsync(int id);
        Task<SuperHero> CreateAsync(SuperHero superHero);
        Task<SuperHero?> UpdateAsync(int id, SuperHero superHero);
        Task<bool> DeleteAsync(int id);
    }
}
