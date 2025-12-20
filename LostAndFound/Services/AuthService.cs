using LostAndFound.Models;
using LostAndFound.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LostAndFound.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User?> AuthenticateAsync(string login, string password)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);
            if (user == null) return null;

            bool isPasswordValid = _passwordHasher.VerifyPassword(user.Password, password);
            if (!isPasswordValid) return null;

            return user;
        }

        public int? GetCurrentUserId()
        {
            int? result;

            if (int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int parsedValue))
            {
                result = parsedValue;
            }
            else
            {
                result = null;
            }
            return result; 
        }

        public bool IsAuthenticated()
        {
            throw new NotImplementedException();
        }

        public async Task SignInAsync(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );
        }

        public async Task SignOutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
