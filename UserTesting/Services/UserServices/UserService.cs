using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data;
using UserManagementSystem.DTOs.Users;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<GetUserDto>> GetAllUsers()
        {
            List<User> dbUsers = await _context.Users.ToListAsync();
            return dbUsers.Select(u => _mapper.Map<GetUserDto>(u)).ToList(); ;
        }

        public async Task<GetUserDto> GetUserById(int id)
        {
            User user = await _context.Users.FindAsync(id);
            return _mapper.Map<GetUserDto>(user);
        }

        public async Task<GetUserDto> AddUser(AddUserDto newUser)
        {

            var user = _mapper.Map<User>(newUser);
            if (await UsernameExists(newUser.UserName))
            {
                throw new ArgumentException("Username is already taken.", nameof(newUser.UserName));
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _context.Entry(user).ReloadAsync();

            return _mapper.Map<GetUserDto>(user);
        }


        public async Task<GetUserDto> DeleteUser(int id)
        {
            User user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<GetUserDto>(user);
        }


        public async Task<GetUserDto> UpdateUser(GetUserDto userDto, int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            if (userDto.FullName != null)
            {
                user.FullName = userDto.FullName;
            }

            if (userDto.UserName != null)
            {
                user.UserName = userDto.UserName;
            }

            if (userDto.Age != 0)
            {
                user.Age = userDto.Age;
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<GetUserDto>(user); ;
        }

        private async Task<bool> UsernameExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }

    }
}
