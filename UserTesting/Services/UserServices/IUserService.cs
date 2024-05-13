using UserManagementSystem.DTOs.Auth;
using UserManagementSystem.DTOs.Users;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.UserServices
{
    public interface IUserService
    {
        Task<List<GetUserDto>> GetAllUsers();
        Task<GetUserDto> GetUserById(string id);
        Task<GetUserDto> AddUser(AddUserDto newUser);
        Task<GetUserDto> UpdateUser(GetUserDto newUser, string id);
        Task<GetUserDto> DeleteUser(string id);
    }
}
