using System.ComponentModel.DataAnnotations;

namespace SnakeGameAPI.Models;

public class GameResult
{
    [Key]
    public int ResultId { get; set; }

    public int UserId { get; set; }

    public int ModeId { get; set; }

    public int Score { get; set; }

    public DateTime PlayedAt { get; set; }

    public User? User { get; set; }

    public GameMode? GameMode { get; set; }
}