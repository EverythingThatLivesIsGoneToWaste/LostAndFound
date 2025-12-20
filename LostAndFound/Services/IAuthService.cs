using LostAndFound.Models;

namespace LostAndFound.Services
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string login, string password);
        Task SignInAsync(User user);
        Task SignOutAsync();
        bool IsAuthenticated();
        int? GetCurrentUserId();
    }
}
