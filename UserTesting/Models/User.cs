using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace UserManagementSystem.Models
{
    public class User
    {
        public int Id { set; get; }
        public string? FullName { set; get; }

        public string UserName { set; get; } 

        public float Age { set; get; }

    }
}
