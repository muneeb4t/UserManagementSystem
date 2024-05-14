namespace UserManagementSystem.Rrepositories.UserRepository
{
    using Azure;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using UserManagementSystem.Models;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> FindUserByEmailAsync(string userEmail)
        {
            return await _userManager.FindByEmailAsync(userEmail);
        }

        public Task<User> FindUserByIdAsync(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public async Task<List<User>> GetUserListAsync(int pageIndex, int pageSize, string orderBy, string orderType)
        {
            List<User> response;
            IQueryable<User> query = _userManager.Users;

            switch (orderBy)
            {
                case "Name":
                    query = orderType == "asc"
                        ? query.OrderBy(u => u.Name)
                        : query.OrderByDescending(u => u.Name);
                    break;
                case "Email":
                    query = orderType == "asc"
                        ? query.OrderBy(u => u.Email)
                        : query.OrderByDescending(u => u.Email);
                    break;
                default:
                    query = orderType == "asc"
                        ? query.OrderBy(u => u.Id)
                        : query.OrderByDescending(u => u.Id);
                    break;
            }

            response = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string passowrd)
        {
            return await _userManager.CreateAsync(user, passowrd);
        }

        public async Task DeletUserAsync(User user)
        {
            await _userManager.DeleteAsync(user);
        }
        public async Task UpdateUserAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }
    }
}
