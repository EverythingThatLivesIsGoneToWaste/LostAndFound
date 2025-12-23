using LostAndFound.Data;
using LostAndFound.Enums;
using LostAndFound.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetUserByLoginAsync(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<bool> UserExistsAsync(string login)
        {
            return await _context.Users.AnyAsync(u => u.Login == login);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.Where(u => u.Role != UserRole.SuperAdmin).ToListAsync();
        }
    }
}
