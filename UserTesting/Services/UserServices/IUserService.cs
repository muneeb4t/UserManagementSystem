using UserManagementSystem.DTOs.Users;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.UserServices
{
    public interface IUserService
    {
        Task<List<GetUserDto>> GetAllUsers();
        Task<GetUserDto> GetUserById(int id);
        Task<GetUserDto> AddUser(AddUserDto newUser);
        Task<GetUserDto> UpdateUser(GetUserDto newUser, int id);
        Task<GetUserDto> DeleteUser(int id);
    }
}
