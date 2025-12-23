using LostAndFound.Models;

namespace LostAndFound.Repositories
{
    public class ItemRepository : IItemRepository
    {
        public async Task AddItemAsync(FoundItem item)
        {
            throw new NotImplementedException();
        }

        public async Task<FoundItem?> GetItemByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ItemExistsAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<User>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
