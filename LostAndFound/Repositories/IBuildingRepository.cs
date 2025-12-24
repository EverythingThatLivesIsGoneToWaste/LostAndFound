using LostAndFound.Models;

namespace LostAndFound.Repositories
{
    public interface IBuildingRepository
    {
        Task<Building?> GetBuildingByIdAsync(int id);
        Task<Building?> GetBuildingByNameAsync(string name);
        Task AddBuildingAsync(Building building);
        Task<List<Building>> GetBuildings();
        Task RemoveBuildingAsync(int id);
    }
}
