using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnakeGameAPI.Data;
using SnakeGameAPI.Models;
using SnakeGameAPI.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace SnakeGameAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly SnakeGameDbContext _context;

    public AccountController(SnakeGameDbContext context)
    {
        _context = context;
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return BadRequest(new
            {
                success = false,
                message = "Passwords do not match."
            });
        }

        string username = request.Username.Trim();

        bool usernameExists = await _context.Users
            .AnyAsync(u => u.Username == username);

        if (usernameExists)
        {
            return BadRequest(new
            {
                success = false,
                message = "Username already exists."
            });
        }

        User user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Username = username,
            PasswordHash = HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        UserSettings settings = new UserSettings
        {
            UserId = user.UserId,
            BackgroundColor = "DarkGreen",
            SnakeSpeed = 0.15
        };

        _context.UserSettings.Add(settings);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Account created successfully.",
            userId = user.UserId,
            firstName = user.FirstName
        });
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        string username = request.Username.Trim();

        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid username or password."
            });
        }

        bool passwordCorrect =
            VerifyPassword(request.Password, user.PasswordHash);

        if (!passwordCorrect)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid username or password."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Login successful.",
            userId = user.UserId,
            firstName = user.FirstName,
            lastName = user.LastName,
            username = user.Username
        });
    }

    private static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            salt,
            100000,
            HashAlgorithmName.SHA256);

        byte[] hash = pbkdf2.GetBytes(32);

        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPassword(
        string password,
        string storedPassword)
    {
        string[] parts = storedPassword.Split(':');

        if (parts.Length != 2)
            return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] storedHash = Convert.FromBase64String(parts[1]);

        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            salt,
            100000,
            HashAlgorithmName.SHA256);

        byte[] hash = pbkdf2.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(
            hash,
            storedHash);
    }
}