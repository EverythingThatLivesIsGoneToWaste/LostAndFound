using LostAndFound.Models;

namespace LostAndFound.Repositories
{
    public interface IItemRepository
    {
        Task<FoundItem?> GetItemByIdAsync(int id);
        Task AddItemAsync(FoundItem item);
        Task<List<FoundItem>> GetItemsAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? searchTerm = null,
            int? roomId = null);
        Task RemoveItemAsync(FoundItem item);
    }
}
