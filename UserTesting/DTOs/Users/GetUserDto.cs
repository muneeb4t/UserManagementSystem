using UserManagementSystem.Models;

namespace UserManagementSystem.DTOs.Users
{
    public class GetUserDto
    {
        public int Id { set; get; }
        public string? FullName { set; get; }
        public string? UserName { set; get; }
        public float Age { set; get; }
    }
}
