using LostAndFound.Models;

namespace LostAndFound.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByLoginAsync(string login);
        Task<User?> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task<bool> UserExistsAsync(string login);
        Task<List<User>> GetUsersAsync();
        Task RemoveUserAsync(User user);
    }
}
