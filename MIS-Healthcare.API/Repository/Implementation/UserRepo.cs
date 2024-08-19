using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Data;
using MIS_Healthcare.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MIS_Healthcare.API.Repository.Implementation
{
    public class UserRepo:iUserRepo
    {
        private readonly HealthcareContext _context;

        public UserRepo(HealthcareContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
