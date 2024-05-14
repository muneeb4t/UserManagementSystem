using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data;
using UserManagementSystem.DTOs.Auth;
using UserManagementSystem.DTOs.Users;
using UserManagementSystem.Models;
using UserManagementSystem.Rrepositories.UserRepository;

namespace UserManagementSystem.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<GetUserDto>> GetAllUsers(int pageIndex, int pageSize, string orderBy, string orderType)
        {
            List<User> dbUsers = await _userRepository.GetUserListAsync(pageIndex, pageSize, orderBy , orderType);
            return dbUsers.Select(user => _mapper.Map<GetUserDto>(user)).ToList(); ;
        }

        public async Task<GetUserDto> GetUserById(string id)
        {
            User user = await _userRepository.FindUserByIdAsync(id);
            return _mapper.Map<GetUserDto>(user);
        }

        public async Task<GetUserDto> AddUser(AddUserDto userDTO)
        {

            if (userDTO is null)
                throw new Exception("Invalid Data");
            User user = _mapper.Map<User>(userDTO);
            var existingUser = await _userRepository.FindUserByEmailAsync(user.Email!);
            if (existingUser != null)
                throw new Exception("User already exist with this email");
            IdentityResult isCreated = await _userRepository.CreateUserAsync(user, userDTO.Password!);
            if (!isCreated.Succeeded)
            {
                var errorMessages = string.Join(Environment.NewLine, isCreated.Errors.Select(e => e.Description));
                throw new Exception($"{Environment.NewLine}{errorMessages}");
            }

            return (_mapper.Map<GetUserDto>(user));
        }

        public async Task<GetUserDto> DeleteUser(string id)
        {
            User user = await _userRepository.FindUserByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeletUserAsync(user);
            }

            return _mapper.Map<GetUserDto>(user);
        }


        public async Task<GetUserDto> UpdateUser(GetUserDto userDto, string id)
        {
            User user = await _userRepository.FindUserByIdAsync(id);
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

            await _userRepository.UpdateUserAsync(user);

            return _mapper.Map<GetUserDto>(user); ;
        }

    }
}
