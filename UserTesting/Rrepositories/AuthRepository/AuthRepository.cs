using Microsoft.AspNetCore.Identity;
using UserManagementSystem.DTOs.Auth;
using UserManagementSystem.Models;
using UserManagementSystem.Rrepositories.AuthRepository;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<User> _userManager;
    public AuthRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }



    public Task<string> Login(LoginDTO user)
    {
        throw new NotImplementedException();
    }

    public async Task<User> FindUserByEmailAsync(string userEmail)
    {
        return await _userManager.FindByEmailAsync(userEmail);
    }

    public Task<User> FindUserByIdAsync(string id)
    {
        return _userManager.FindByIdAsync(id);
    }

    public async Task<IdentityResult> CreateAsync(User user , string passowrd)
    {
        return await _userManager.CreateAsync(user, passowrd);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
    
    public async Task<bool> IsEmailConfirmedAsync(User user)
    {
        return await _userManager.IsEmailConfirmedAsync(user);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<IdentityResult> ConfirmEmail(User user, string token)
    {
        return await _userManager.ConfirmEmailAsync(user , token);
    }
}
