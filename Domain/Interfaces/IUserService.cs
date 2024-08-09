using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task CreateUser(UserRegistrationModel user);
        Task UpdateUser(int id, UserUpdateModel updatedUser);
        Task DeleteUser(int id);
    }
}
