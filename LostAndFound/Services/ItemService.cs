using LostAndFound.DTOs;
using LostAndFound.Models;
using LostAndFound.Exceptions;
using LostAndFound.Repositories;

namespace LostAndFound.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<ItemService> _logger;

        public ItemService(
            IItemRepository itemRepository, 
            IRoomRepository roomRepository,
            ILogger<ItemService> logger)
        {
            _itemRepository = itemRepository;
            _roomRepository = roomRepository;
            _logger = logger;
        }

        public async Task<ItemDto> AddItemAsync(CreateItemDto itemDto)
        {
            if (itemDto.FoundAtUtc > DateTime.UtcNow)
                throw new ValidationException("Date cannot be in the future");

            var dateUtc = DateTime.SpecifyKind(
                itemDto.FoundAtUtc,
                DateTimeKind.Utc
                );

            var room = await _roomRepository.GetRoomByIdAsync(itemDto.RoomId)
                ?? throw new NotFoundException($"Room with id {itemDto.RoomId} not found");

            var item = new FoundItem
            {
                Name = itemDto.Name,
                Info = itemDto.Info,
                FoundAtUtc = dateUtc,
                Room = room
            };

            await _itemRepository.AddItemAsync(item);
            _logger.LogInformation("Item {Name} created", item.Name);

            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Info = item.Info,
                FoundAtUtc = item.FoundAtUtc,
                RoomId = item.RoomId
            };
        }

        public async Task RemoveItemAsync(int id)
        {
            var item = await _itemRepository.GetItemByIdAsync(id) 
                ?? throw new NotFoundException($"Item with id {id} not found");

            await _itemRepository.RemoveItemAsync(item);
            _logger.LogInformation("Item {Name} deleted", item.Name);
        }

        public async Task<FoundItem> GetItemByIdAsync(int id)
        {
            var item = await _itemRepository.GetItemByIdAsync(id);
            return item ?? throw new NotFoundException($"Item with id {id} not found");
        }
    }
}
