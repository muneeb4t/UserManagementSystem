using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.DTOs.Auth;
using UserManagementSystem.DTOs.Users;
using UserManagementSystem.Services.AuthService;

namespace UserManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO userDTO)
        {
            try
            {
                var response = await _authService.Register(userDTO);
                return this.Ok(response, "User Registration Successfull");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message, "");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO userDTO)
        {
            try
            {
                var response = await _authService.Login(userDTO);
                return this.Ok("User LoggedIn Successsfully" , new { accesstoken =  response });
            }

            catch (Exception ex)
            {
                return this.BadRequest(ex.Message, "");
            }
        }

    }
}
