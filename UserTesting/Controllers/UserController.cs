using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.DTOs.Auth;
using UserManagementSystem.DTOs.Users;
using UserManagementSystem.Models;
using UserManagementSystem.Services.UserServices;

namespace UserManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : BaseController
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, int pageSize = 10, string orderBy = "userName" , string orderType = "asc")
        {
            try
            { 
                var users = await _userService.GetAllUsers(pageIndex, pageSize , orderBy , orderType);
                return Ok("Users retrieved successfully.", users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message , "");
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            try
            {
                var user = await _userService.GetUserById(Id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                return Ok("User retrieved successfully.", user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message , "");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto user)
        {
            try
            {
                return Ok("User Created Successfully", await _userService.AddUser(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message , "");
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser(GetUserDto user, string Id)
        {
            try
            {
                var responce = await _userService.UpdateUser(user, Id);
                if (responce == null)
                {
                    return NotFound("User not found.");
                }
                return Ok("User Updated Successfully", responce);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message , "");
            }

        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            try
            {
                var responce = await _userService.DeleteUser(Id);
                return Ok("User Deleted Successfully" , "");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message , "");
            }
        }

    }
}

