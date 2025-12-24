using LostAndFound.Data;
using LostAndFound.Models;
using LostAndFound.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new NotFoundException($"Room with id {id} not found");
        }

        public async Task<List<Room>> GetRoomsByBuildingIdAsync(int buildingId)
        {
            return await _context.Rooms
                .Include(r => r.Building)
                .Where(r => r.BuildingId == buildingId)
                .ToListAsync();
        }

        public async Task AddRoomAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRoomAsync(int id)
        {
            var room = await GetRoomByIdAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }
}
