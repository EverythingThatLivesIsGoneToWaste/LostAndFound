using LostAndFound.Models;

namespace LostAndFound.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByLoginAsync(string login);
        Task AddUserAsync(User user);
        Task<bool> UserExistsAsync(string login);
        Task<List<User>> GetUsersAsync();
    }
}
