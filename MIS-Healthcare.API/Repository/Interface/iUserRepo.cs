using MIS_Healthcare.API.Data.Models;

namespace MIS_Healthcare.API.Repository.Interface
{
    public interface iUserRepo
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
    }
}
