using Microsoft.AspNetCore.Identity;
using UserManagementSystem.Models;

namespace UserManagementSystem.Rrepositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> FindUserByEmailAsync(string userEmail);
        Task<List<User>> GetUserListAsync(int pageIndex, int pageSize, string orderBy, string orderType);
        Task<User> FindUserByIdAsync(string id);
        Task<IdentityResult> CreateUserAsync(User user, string passowrd);
        Task DeletUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
