using LostAndFound.Data;
using LostAndFound.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<FoundItem?> GetItemByIdAsync(int id)
        {
            return await _context.FoundItems.FindAsync(id);
        }

        public async Task AddItemAsync(FoundItem item)
        {
            await _context.FoundItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FoundItem>> GetItemsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? searchTerm = null,
            int? roomId = null)
        {
            var query = _context.FoundItems.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(i => i.FoundAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(i => i.FoundAt <= toDate.Value);

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(i =>
                    i.Name.Contains(searchTerm) ||
                    i.Info.Contains(searchTerm));

            if (roomId.HasValue)
                query = query.Where(i => i.RoomId == roomId.Value);

            return await query.ToListAsync();
        }

        public async Task RemoveItemAsync(FoundItem item)
        {
            _context.FoundItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
