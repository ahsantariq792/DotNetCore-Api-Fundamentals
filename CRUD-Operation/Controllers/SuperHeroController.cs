using CRUD_Operation.Entities;
using CRUD_Operation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Operation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly ISuperHeroService _superHeroService;

        public SuperHeroController(ISuperHeroService superHeroService)
        {
            _superHeroService = superHeroService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            var superHeroes = await _superHeroService.GetAllAsync();
            return Ok(superHeroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetById(int id)
        {
            var superHero = await _superHeroService.GetByIdAsync(id);
            if (superHero == null)
                return NotFound();
            return Ok(superHero);
        }

        [HttpPost]
        public async Task<ActionResult<SuperHero>> Add(SuperHero superHero)
        {
            var created = await _superHeroService.CreateAsync(superHero);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SuperHero>> Update(int id, SuperHero superHero)
        {
            if (id != superHero.Id)
                return BadRequest();

            var updated = await _superHeroService.UpdateAsync(id, superHero);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _superHeroService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
