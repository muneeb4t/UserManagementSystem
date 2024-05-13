using AutoMapper;
using UserManagementSystem.DTOs.Auth;
using UserManagementSystem.DTOs.Users;
using UserManagementSystem.Models;

namespace UserManagementSystem
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>().ReverseMap();
            CreateMap<User, AddUserDto>().ReverseMap();
            CreateMap<User, RegisterDTO>().ReverseMap();
        }
    }
}
