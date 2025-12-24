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
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, 
            IUserService userService, 
            ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _userService = userService;
            _logger = logger;
        }

        // GET: users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();
                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Login = u.Login,
                    FullName = u.FullName,
                    Role = u.Role
                }).ToList();

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        // GET: users/[int]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {UserId}", id);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        // POST: users
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromForm] CreateUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.AddUserAsync(model);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (AlreadyExistsException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        // DELETE: users/[int]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.RemoveUserAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
