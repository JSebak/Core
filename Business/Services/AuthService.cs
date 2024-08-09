using Business.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace Business.Services
{
    public class AuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public AuthService(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<string?> Login(UserLoginModel userLoginModel)
        {
            var user = await _userService.GetUserByEmail(userLoginModel.Email);
            if (user == null) throw new ArgumentNullException("Invalid Email or Password");
            if (BCrypt.Net.BCrypt.Verify(userLoginModel.Password, user.Password))
            {
                var token = _tokenService.GenerateToken(user);
                return token;
            }
            else
            {
                throw new Exception("Invalid Email or Password");
            }
        }
    }
}
