using Lab3_FullstackAuctionWebsite.Data;
using Lab3_FullstackAuctionWebsite.Data.Entities;
using Lab3_FullstackAuctionWebsite.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab3_FullstackAuctionWebsite.Data.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        // -------------------- CREATE USER --------------------

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // -------------------- DELETE USER --------------------
        // For auction system we SHOULD NOT hard delete.

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return;

            // Soft delete instead of removing
            user.IsActive = false;

            await _context.SaveChangesAsync();
        }

        // -------------------- GET ALL USERS --------------------

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // -------------------- GET USER BY ID --------------------

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.UserId == id);
        }

        // -------------------- GET USER BY USERNAME --------------------

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == userName);
        }

        // -------------------- UPDATE USER --------------------

        public async Task UpdateUserAsync(User userUpdated)
        {
            var userOrg = await _context.Users
                .SingleOrDefaultAsync(u => u.UserId == userUpdated.UserId);

            if (userOrg == null)
                return;

            _context.Entry(userOrg).CurrentValues.SetValues(userUpdated);

            await _context.SaveChangesAsync();
        }

        // -------------------- LOGIN --------------------

        public async Task<User?> LoginAsync(string userName)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u =>
                    u.UserName == userName &&
                    u.IsActive == true);
        }
    }
}

