using Microsoft.EntityFrameworkCore;
using SnakeGameAPI.Models;

namespace SnakeGameAPI.Data;

public class SnakeGameDbContext : DbContext
{
    public SnakeGameDbContext(
        DbContextOptions<SnakeGameDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<UserSettings> UserSettings => Set<UserSettings>();

    public DbSet<GameMode> GameModes => Set<GameMode>();

    public DbSet<GameResult> GameResults => Set<GameResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("Users")
            .HasKey(u => u.UserId);

        modelBuilder.Entity<UserSettings>()
            .ToTable("UserSettings")
            .HasKey(s => s.SettingsId);

        modelBuilder.Entity<GameMode>()
            .ToTable("GameModes")
            .HasKey(m => m.ModeId);

        modelBuilder.Entity<GameResult>()
            .ToTable("GameResults")
            .HasKey(r => r.ResultId);

        modelBuilder.Entity<UserSettings>()
            .HasOne(s => s.User)
            .WithOne(u => u.UserSettings)
            .HasForeignKey<UserSettings>(s => s.UserId);

        modelBuilder.Entity<GameResult>()
            .HasOne(r => r.User)
            .WithMany(u => u.GameResults)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<GameResult>()
            .HasOne(r => r.GameMode)
            .WithMany(m => m.GameResults)
            .HasForeignKey(r => r.ModeId);
    }
}