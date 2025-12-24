using LostAndFound.Models;
using LostAndFound.DTOs;

namespace LostAndFound.Services
{
    public interface IUserService
    {
        Task<User> AddUserAsync(CreateUserDto model);
        Task RemoveUserAsync(int id);
        Task<User> GetUserByIdAsync(int id);
    }
}
