using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly IConfiguration _config;

    public HealthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var connString = _config.GetConnectionString("DefaultConnection");

        try
        {
            using var conn = new SqlConnection(connString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("SELECT 1", conn);
            var result = await cmd.ExecuteScalarAsync();

            return Ok(new { status = "Healthy", db = "Connected", result });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new { status = "Unhealthy", error = ex.Message });
        }
    }
}
