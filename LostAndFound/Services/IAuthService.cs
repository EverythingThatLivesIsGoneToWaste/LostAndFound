using LostAndFound.Models;

namespace LostAndFound.Services
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string login, string password);
        Task LoginAsync(User user);
        Task LogoutAsync();
    }
}
