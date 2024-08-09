using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                return await _userRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<User> GetUserById(int id)
        {
            try
            {
                return _userRepository.GetById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<User> GetUserByEmail(string email)
        {
            try
            {
                return _userRepository.GetByEmail(email);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task CreateUser(UserRegistrationModel userToRegister)
        {
            if (string.IsNullOrWhiteSpace(userToRegister.UserName) ||
                string.IsNullOrWhiteSpace(userToRegister.Email) ||
                string.IsNullOrWhiteSpace(userToRegister.Password) ||
                string.IsNullOrWhiteSpace(userToRegister.Role))
            {
                throw new ArgumentException("All fields are required");
            }

            try
            {
                var existingUser = await _userRepository.GetByEmail(userToRegister.Email);
                var existingUserByUsername = await _userRepository.GetByUserName(userToRegister.UserName);

                if (existingUser != null || existingUserByUsername != null)
                    throw new InvalidDataException("User already exists");

                var user = new User(userToRegister.UserName, userToRegister.Password, userToRegister.Email, userToRegister.Role);
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userToRegister.Password);
                user.UpdatePassword(hashedPassword);

                await _userRepository.Add(user);
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var message = $"An error occurred while registering the user with Email {userToRegister.Email}.";
                _logger.LogError(ex, message);
                throw new Exception(message, ex);
            }
        }

        public async Task UpdateUser(int id, UserUpdateModel updatedUser)
        {
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                    throw new KeyNotFoundException("User not found");

                var isUpdated = false;

                if (!string.IsNullOrEmpty(updatedUser.Email) && user.Email != updatedUser.Email)
                {
                    user.UpdateEmail(updatedUser.Email);
                    isUpdated = true;
                }
                if (!string.IsNullOrEmpty(updatedUser.UserName) && user.Username != updatedUser.UserName)
                {
                    user.UpdateUserName(updatedUser.UserName);
                    isUpdated = true;
                }
                if (!string.IsNullOrEmpty(updatedUser.Password) && !BCrypt.Net.BCrypt.Verify(updatedUser.Password, user.Password))
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password);
                    user.UpdatePassword(hashedPassword);
                    isUpdated = true;
                }
                if (!string.IsNullOrEmpty(updatedUser.Role) && user.Role != updatedUser.Role)
                {
                    user.ChangeRole(updatedUser.Role);
                    isUpdated = true;
                }

                if (isUpdated)
                {
                    await _userRepository.Update(user);
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "User with ID {UserId} not found.", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user with ID {UserId}.", id);
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }

        public async Task DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                    throw new KeyNotFoundException("User not found");
                await _userRepository.Delete(id);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
