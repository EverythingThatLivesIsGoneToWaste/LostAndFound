using LostAndFound.Configuration;
using LostAndFound.Data;
using LostAndFound.Enums;
using LostAndFound.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LostAndFound.Services;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _hasher;
    private readonly SeedDataSettings _seedSettings;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        AppDbContext context,
        IPasswordHasher hasher,
        IOptions<SeedDataSettings> seedOptions,
        ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _hasher = hasher;
        _seedSettings = seedOptions.Value;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Starting database seeding...");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await SeedSuperadminAsync();
            await SeedDefaultBuildingAndRoomAsync();

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Database seeded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding database");
            throw;
        }
    }

    private async Task SeedSuperadminAsync()
    {
        if (!await _context.Users.AnyAsync(u => u.Role == UserRole.SuperAdmin))
        {
            var admin = new User
            {
                Login = _seedSettings.Superadmin.Login,
                FullName = _seedSettings.Superadmin.FullName,
                Password = _hasher.HashPassword(_seedSettings.Superadmin.Password),
                Role = UserRole.SuperAdmin
            };
            await _context.Users.AddAsync(admin);
            _logger.LogInformation("Superadmin user created: {Login}", admin.Login);
        }
    }

    private async Task SeedDefaultBuildingAndRoomAsync()
    {
        var building = await _context.Buildings
            .FirstOrDefaultAsync(b => b.Name == _seedSettings.DefaultBuilding.Name);

        if (building == null)
        {
            building = new Building
            {
                Name = _seedSettings.DefaultBuilding.Name,
                Description = _seedSettings.DefaultBuilding.Description,
                Address = _seedSettings.DefaultBuilding.Address
            };
            await _context.Buildings.AddAsync(building);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Default building created: {Name}", building.Name);
        }

        if (!await _context.Rooms.AnyAsync(r => r.Name == _seedSettings.DefaultRoom.Name))
        {
            var room = new Room
            {
                Name = _seedSettings.DefaultRoom.Name,
                Floor = _seedSettings.DefaultRoom.Floor,
                BuildingId = building.Id
            };
            await _context.Rooms.AddAsync(room);
            _logger.LogInformation("Default room created: {Name}", room.Name);
        }
    }
}
