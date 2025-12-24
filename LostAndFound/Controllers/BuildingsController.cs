using LostAndFound.DTOs;
using LostAndFound.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<UsersController> _logger;

        public BuildingsController(IBuildingRepository buildingRepository,
            IRoomRepository roomRepository,
            ILogger<UsersController> logger)
        {
            _buildingRepository = buildingRepository;
            _roomRepository = roomRepository;
            _logger = logger;
        }

        // GET: buildings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildingDto>>> GetBuildings()
        {
            try
            {
                var buildings = await _buildingRepository.GetBuildings();
                var buildingDtos = buildings.Select(b => new BuildingDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    Address = b.Address,
                    Rooms = [.. b.Rooms.Select(r => new RoomDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Floor = r.Floor
                    })]
                }).ToList();

                return Ok(buildingDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting buildings");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("{id:int}/rooms")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetBuildingRooms(int id)
        {
            try
            {
                var rooms = await _roomRepository.GetRoomsByBuildingIdAsync(id);
                var roomDtos = rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Floor = r.Floor
                }).ToList();

                return Ok(roomDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting rooms");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
