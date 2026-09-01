using System.ComponentModel.DataAnnotations;

namespace SnakeGameAPI.Models;

public class UserSettings
{
    [Key]
    public int SettingsId { get; set; }

    public int UserId { get; set; }

    public string BackgroundColor { get; set; } = "DarkGreen";

    public double SnakeSpeed { get; set; } = 0.15;

    public User? User { get; set; }
}