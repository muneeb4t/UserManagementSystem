using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.DTOs.Users;
using UserManagementSystem.Models;
using UserManagementSystem.Services.UserServices;

namespace UserManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return this.Ok("Users retrieved successfully.", users);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var user = await _userService.GetUserById(Id);
                if (user == null)
                {
                    return this.NotFound<User>("User not found.");
                }
                return this.Ok("User retrieved successfully.", user);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto user)
        {
            try
            {
                return this.Ok("User Created Successfully", await _userService.AddUser(user));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message , "");
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser(GetUserDto user, int Id)
        {
            try
            {
                var responce = await _userService.UpdateUser(user, Id);
                if (responce == null)
                {
                    return this.NotFound<User>("User not found.");
                }
                return this.Ok("User Updated Successfully", responce);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            try
            {
                var responce = await _userService.DeleteUser(Id);
                return this.Ok("User Deleted Successfully" , "");
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

    }
}

