using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UsersService.Application;
using UsersService.Application.DTO;
using UsersService.Domain.Models;
using UsersService.Domain.Repositories;

namespace StudentSwapApp.API
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IRepository _repository;
        private readonly HttpClient _httpClient;
        private readonly ImanageUserService _userService;

        public UserController(IRepository repository, HttpClient httpClient)
        {
            _repository = repository;
            _httpClient = httpClient;
        }
        [HttpPut("{username}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserModel updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            try
            {
                var user = await _repository.UpdateUserAsync(updatedUser);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with ID {id} not found.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE User - only for role 'Admin'
        [HttpDelete("{username}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var deleted = await _repository.DeleteUserAsync(username);

            if (!deleted)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            return NoContent();
        }
        [Authorize]
        [HttpPatch("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _userService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
            return NoContent();
        }

        [Authorize]
        [HttpPatch("updateAddress")]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _userService.UpdateAddressAsync(userId, request.City, request.Street, request.Country);
            return NoContent();
        }



        [HttpGet("getUserByID")]
        public async Task<IActionResult> GetUserByID(string id)
        {
            try
            {
                var user = await _repository.GetUserAsync(id);

                if (user == null)
                {
                    return NotFound("nie znaleziono");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
            
                return StatusCode(500);
            }
        }
    }
}
