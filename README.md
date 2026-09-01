# 🐍 Snake Game

A classic Snake game built with **Unity 6** and backed by an **ASP.NET Core Web API** for user authentication and data management.

## Project Structure

```
Snake-Game/
├── SnakeGame/          # Unity 6 game client
│   ├── Assets/
│   │   ├── Scripts/    # C# game scripts
│   │   ├── Scenes/     # Game, Login, Register, Welcome, Settings, ModeSelect
│   │   ├── Prefabs/    # Snake body prefab
│   │   ├── Sprites/    # Game sprites
│   │   └── Audio/      # Sound effects
│   ├── Packages/
│   └── ProjectSettings/
│
└── SnakeGameAPI/       # ASP.NET Core 8 Web API
    └── SnakeGameAPI/
        ├── Controllers/    # Account & Database controllers
        ├── Models/         # User, GameMode, GameResult, DTOs
        └── Program.cs      # API entry point
```

## Features

- 🎮 Classic snake gameplay with arrow key / WASD controls
- 🔐 User registration & login with hashed passwords (PBKDF2-SHA256)
- 🏆 Score tracking with game-over / restart flow
- ⚙️ User settings (background color, snake speed)
- 🎯 Multiple game modes
- 📡 REST API with Swagger documentation

## Tech Stack

| Layer     | Technology                          |
|-----------|-------------------------------------|
| Game      | Unity 6 (6000.5.8f1), C#           |
| API       | ASP.NET Core 8, Entity Framework Core |
| Database  | SQL Server                          |
| Auth      | PBKDF2-SHA256 password hashing      |

## Getting Started

### API

1. Update the connection string in `SnakeGameAPI/SnakeGameAPI/appsettings.json`
2. Run EF migrations:
   ```bash
   dotnet ef database update
   ```
3. Start the API:
   ```bash
   cd SnakeGameAPI/SnakeGameAPI
   dotnet run
   ```
4. Swagger UI available at `https://localhost:7216/swagger`

### Game

1. Open `SnakeGame/` folder in **Unity 6**
2. Open the `Login` scene from `Assets/Scenes/`
3. Make sure the API is running
4. Hit **Play** ▶️

## Controls

| Key              | Action     |
|------------------|------------|
| `↑` / `W`        | Move Up    |
| `↓` / `S`        | Move Down  |
| `←` / `A`        | Move Left  |
| `→` / `D`        | Move Right |

## License

This project is for personal / educational use.
