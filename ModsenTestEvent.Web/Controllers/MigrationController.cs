namespace ModsenTestEvent.Web.Controllers;

[Route("api/migrations")] 
[ApiController]
public class MigrationController : ControllerBase
{
    private readonly DataContext _dbContext;
    private readonly ILogger<MigrationController> _logger;

    public MigrationController(DataContext dbContext, ILogger<MigrationController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Выполняет миграции базы данных.
    /// </summary>
    [HttpPost("apply")]
    public async Task<IActionResult> ApplyMigrations()
    {
        try
        {
            _logger.LogInformation("Запуск миграций...");
            await _dbContext.Database.MigrateAsync();
            _logger.LogInformation("Миграции успешно применены.");
            return Ok("Миграции успешно применены.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при выполнении миграций: {ex.Message}");
            return StatusCode(500, $"Ошибка при выполнении миграций: {ex.Message}");
        }
    }
}