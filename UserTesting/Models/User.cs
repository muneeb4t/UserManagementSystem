using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string? Name { set; get; }
        public float Age { set; get; }

    }
}
