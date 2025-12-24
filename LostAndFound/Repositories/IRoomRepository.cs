using LostAndFound.Models;

namespace LostAndFound.Repositories
{
    public interface IRoomRepository
    {
        Task<Room> GetRoomByIdAsync(int id);
        Task<List<Room>> GetRoomsByBuildingIdAsync(int buildingId);
        Task AddRoomAsync(Room room);
        Task RemoveRoomAsync(int id);
    }
}
