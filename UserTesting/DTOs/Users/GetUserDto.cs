using System.ComponentModel.DataAnnotations;
using UserManagementSystem.Models;

namespace UserManagementSystem.DTOs.Users
{
    public class GetUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

    }
}
