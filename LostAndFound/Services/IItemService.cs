using LostAndFound.DTOs;
using LostAndFound.Models;

namespace LostAndFound.Services
{
    public interface IItemService
    {
        Task<ItemDto> AddItemAsync(CreateItemDto model);
        Task RemoveItemAsync(int id);
        Task<FoundItem> GetItemByIdAsync(int id);
    }
}
