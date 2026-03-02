using CRUD_Operation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Operation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly DataContext _context;

        public HealthController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Checks if the application can connect to the database.
        /// </summary>
        [HttpGet("db")]
        public async Task<ActionResult> CheckDatabaseConnection(CancellationToken cancellationToken = default)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync(cancellationToken);
                try
                {
                    var databaseName = connection.Database;
                    var recordCount = await _context.SuperHeroes.CountAsync(cancellationToken);
                    return Ok(new
                    {
                        status = "Healthy",
                        database = databaseName,
                        server = connection.DataSource,
                        superHeroesCount = recordCount,
                        message = $"Connected to database '{databaseName}' on server '{connection.DataSource}' ({recordCount} SuperHeroes)"
                    });
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(503, new
                {
                    status = "Unhealthy",
                    database = "Connection failed",
                    error = ex.Message
                });
            }
        }
    }
}
