using LostAndFound.Models;

namespace LostAndFound.Repositories
{
    public interface IItemRepository
    {
        Task<FoundItem?> GetItemByNameAsync(string name);
        Task AddItemAsync(FoundItem item);
        Task<bool> ItemExistsAsync(int id);
        Task<List<User>> GetItemsAsync();
    }
}
