using LostAndFound.Data;
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
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByLoginAsync(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<bool> UserExistsAsync(string login)
        {
            throw new NotImplementedException();
        }
    }
}
