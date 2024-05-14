


using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UserManagementSystem.DTOs.Auth;
using UserManagementSystem.Models;

namespace UserManagementSystem.Rrepositories.AuthRepository
{
    public interface IAuthRepository
    {
        Task<IdentityResult> CreateAsync(User user , string password);
        Task<string> Login(LoginDTO user);
        Task<User> FindUserByEmailAsync(string userEmail);
        Task<User> FindUserByIdAsync(string id);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> IsEmailConfirmedAsync(User user);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmail(User user, string token);

    }
}
