using System.ComponentModel.DataAnnotations;

namespace SnakeGameAPI.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string Username { get; set; } = "";

    public string PasswordHash { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public UserSettings? UserSettings { get; set; }

    public ICollection<GameResult> GameResults { get; set; }
        = new List<GameResult>();
}