using LostAndFound.Data;
using LostAndFound.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly AppDbContext _context;

        public BuildingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Building?> GetBuildingByIdAsync(int id)
        {
            return await _context.Buildings.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<Building?> GetBuildingByNameAsync(string name)
        {
            return await _context.Buildings.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task AddBuildingAsync(Building building)
        {
            await _context.Buildings.AddAsync(building);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Building>> GetBuildings()
        {
            return await _context.Buildings
                .Include(b => b.Rooms)
                .ToListAsync();
        }

        public async Task RemoveBuildingAsync(int id)
        {
            var building = await GetBuildingByIdAsync(id);
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
        }
    }
}
