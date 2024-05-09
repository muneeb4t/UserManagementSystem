using UserManagementSystem.Models;

namespace UserManagementSystem.DTOs.Users
{
    public class AddUserDto
    {
        public string FullName { set; get; }
        public string UserName { set; get; }
        public float Age { set; get; }
    }
}
