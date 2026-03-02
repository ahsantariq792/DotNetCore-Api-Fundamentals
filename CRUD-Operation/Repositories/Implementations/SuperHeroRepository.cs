using CRUD_Operation.Data;
using CRUD_Operation.Entities;
using CRUD_Operation.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Operation.Repositories.Implementations
{
    public class SuperHeroRepository : ISuperHeroRepository
    {
        private readonly DataContext _context;

        public SuperHeroRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SuperHero>> GetAllAsync()
        {
            return await _context.SuperHeroes.AsNoTracking().ToListAsync();
        }

        public async Task<SuperHero?> GetByIdAsync(int id)
        {
            return await _context.SuperHeroes.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<SuperHero> AddAsync(SuperHero superHero)
        {
            _context.SuperHeroes.Add(superHero);
            await _context.SaveChangesAsync();
            return superHero;
        }

        public async Task<SuperHero?> UpdateAsync(int id, SuperHero superHero)
        {
            var existing = await _context.SuperHeroes.FindAsync(id);
            if (existing == null)
                return null;

            existing.Name = superHero.Name;
            existing.FirstName = superHero.FirstName;
            existing.LastName = superHero.LastName;
            existing.Place = superHero.Place;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var superHero = await _context.SuperHeroes.FindAsync(id);
            if (superHero == null)
                return false;

            _context.SuperHeroes.Remove(superHero);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
