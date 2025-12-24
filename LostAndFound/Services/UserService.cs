using LostAndFound.DTOs;
using LostAndFound.Exceptions;
using LostAndFound.Models;
using LostAndFound.Repositories;

namespace LostAndFound.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UserService> _logger;

        public UserService (
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher, 
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }
        public async Task<User> AddUserAsync(CreateUserDto model)
        {
            if (await _userRepository.UserExistsAsync(model.Login))
                throw new AlreadyExistsException($"User '{model.Login}' already exists");

            var user = new User
            {
                Login = model.Login,
                FullName = model.FullName,
                Password = _passwordHasher.HashPassword(model.Password),
                Role = model.Role,
            };

            await _userRepository.AddUserAsync(user);
            _logger.LogInformation("User {Login} created", user.Login);

            return user;
        }

        public async Task RemoveUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id) 
                ?? throw new NotFoundException($"User with id {id} not found");

            await _userRepository.RemoveUserAsync(user);
            _logger.LogInformation("User {Login} deleted", user.Login);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user ?? throw new NotFoundException($"User with id {id} not found");
        }
    }
}
