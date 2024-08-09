using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (_context.Users.Any(u => u.Email == user.Email))
                throw new InvalidOperationException("User with the same email already exists");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid user ID", nameof(id));

            var user = _context.Users.Find(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid user ID", nameof(id));

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                if (_context.Users.Any(u => u.Id == user.Id))
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException("User not found");
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user.");
                throw new Exception("A database error occurred while updating the user.", ex);
            }
        }


        public async Task<User> GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Invalid user email", nameof(email));

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new KeyNotFoundException("There's no user registered with this email");

            return user;
        }

        public async Task<User> GetByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("Invalid username", nameof(userName));

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == userName);
            if (user == null)
                throw new KeyNotFoundException("There's no user registered with this username");

            return user;
        }
    }
}
