using LostAndFound.DTOs;
using LostAndFound.Exceptions;
using LostAndFound.Repositories;
using LostAndFound.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemService _itemService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemRepository itemRepository,
            IItemService itemService,
            ILogger<ItemsController> logger)
        {
            _itemRepository = itemRepository;
            _itemService = itemService;
            _logger = logger;
        }

        // GET: items
        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult> GetItems([FromQuery] FilterItemsDto filter)
        {
            try
            {
                var items = await _itemRepository.GetItemsAsync(
                    fromDate: filter.FromDate,
                    toDate: filter.ToDate,
                    searchTerm: filter.Search,
                    roomId: filter.RoomId);

                var itemDtos = items.Select(i => new ItemDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Info = i.Info,
                    FoundAt = i.FoundAt,
                    RoomId = i.RoomId
                }).ToList();

                return Ok(itemDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting items");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        // GET: items/[int]
        [HttpGet("{id:int}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<ItemDto>> GetItem(int id)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);
                return Ok(item);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting item {ItemId}", id);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        // POST: items
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateItem([FromQuery] CreateItemDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var item = await _itemService.AddItemAsync(model);
                return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating item");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        // DELETE: items/[int]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                await _itemService.RemoveItemAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting item {ItemId}", id);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
