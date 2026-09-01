
using System.ComponentModel.DataAnnotations;

namespace SnakeGameAPI.Models;

public class GameMode
{
    [Key]
    public int ModeId { get; set; }

    public string ModeName { get; set; } = "";

    public string? Description { get; set; }

    public ICollection<GameResult> GameResults { get; set; }
        = new List<GameResult>();
}