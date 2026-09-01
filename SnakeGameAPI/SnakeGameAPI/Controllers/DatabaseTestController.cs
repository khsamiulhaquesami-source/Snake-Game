using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnakeGameAPI.Data;

namespace SnakeGameAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseTestController : ControllerBase
{
    private readonly SnakeGameDbContext _context;

    public DatabaseTestController(SnakeGameDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> TestDatabase()
    {
        try
        {
            bool connected = await _context.Database.CanConnectAsync();

            if (connected)
            {
                return Ok(new
                {
                    success = true,
                    message = "SnakeGameDB connection successful!"
                });
            }

            return StatusCode(500, new
            {
                success = false,
                message = "Could not connect to SnakeGameDB."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = ex.Message
            });
        }
    }
}