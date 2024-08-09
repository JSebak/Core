using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
        {
            try
            {
                var token = await _authService.Login(loginModel);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1)
                };
                Response.Cookies.Append("AuthToken", token, cookieOptions);
                return Ok(new { Token = token });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                Response.Cookies.Delete("AuthToken");
                return Ok(new { message = "Logout successful" });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
