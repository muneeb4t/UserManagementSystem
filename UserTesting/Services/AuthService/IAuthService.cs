using UserManagementSystem.DTOs.Auth;
using UserManagementSystem.DTOs.Users;

namespace UserManagementSystem.Services.AuthService
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDTO user);
        Task<string> Login(LoginDTO user);
        Task<string> ConfirmEmail(string userId, string token);
    }
}
