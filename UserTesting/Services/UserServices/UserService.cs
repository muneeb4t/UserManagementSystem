using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data;
using UserManagementSystem.DTOs.Auth;
using UserManagementSystem.DTOs.Users;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<GetUserDto>> GetAllUsers()
        {
            List<User> dbUsers = await _userManager.Users.ToListAsync();
            return dbUsers.Select(user => _mapper.Map<GetUserDto>(user)).ToList(); ;
        }

        public async Task<GetUserDto> GetUserById(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<GetUserDto>(user);
        }

        public async Task<GetUserDto> AddUser(AddUserDto userDTO)
        {

            if (userDTO is null)
                throw new Exception("Invalid Data");
            User user = _mapper.Map<User>(userDTO);
            var existingUser = await _userManager.FindByEmailAsync(user.Email!);
            if (existingUser != null)
                throw new Exception("User already exist with this email");
            var isCreated = await _userManager.CreateAsync(user, userDTO.Password!);
            if (!isCreated.Succeeded)
            {
                var errorMessages = string.Join(Environment.NewLine, isCreated.Errors.Select(e => e.Description));
                throw new Exception($"{Environment.NewLine}{errorMessages}");
            }

            return (_mapper.Map<GetUserDto>(user));
        }


        public async Task<GetUserDto> DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return _mapper.Map<GetUserDto>(user);
        }


        public async Task<GetUserDto> UpdateUser(GetUserDto userDto, string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            if (!string.IsNullOrEmpty(userDto.Name))
            {
                user.Name = userDto.Name;
            }

            if (!string.IsNullOrEmpty(userDto.Username))
            {
                user.UserName = userDto.Username;
            }

            await _userManager.UpdateAsync(user);

            return _mapper.Map<GetUserDto>(user); ;
        }

        //private async Task<bool> UsernameExists(string username)
        //{
        //    return await _context.Users.AnyAsync(u => u.UserName == username);
        //}

    }
}
