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

        public UserController(IRepository repository, HttpClient httpClient, ImanageUserService userService)
        {
            _repository = repository;
            _httpClient = httpClient;
            _userService = userService;
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

      
        [HttpDelete("{username}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var deleted = await _repository.DeleteUserAsync(username);

            if (!deleted)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            return Ok();
        }
        [Authorize]
        [HttpPatch("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            Console.WriteLine("Entered ChangePassword action");

            if (User == null)
            {
                Console.WriteLine("User is null");
                return Unauthorized("User is null");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                Console.WriteLine("User ID claim is missing");
                return Unauthorized("User ID claim missing.");
            }

            Console.WriteLine($"User ID claim found: {userIdClaim.Value}");

            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                Console.WriteLine("User ID claim is invalid");
                return BadRequest("Invalid user ID claim.");
            }

            if (request == null)
            {
                Console.WriteLine("Request body is null");
                return BadRequest("Request body is null.");
            }

            Console.WriteLine($"CurrentPassword: {request.CurrentPassword}");
            Console.WriteLine($"NewPassword: {request.NewPassword}");

            await _userService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);

            Console.WriteLine("Password change completed");
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
        public async Task<IActionResult> GetUserByID(int id)
        {
            try
            {
                var user = await _repository.GetUserByIdAsync(id);

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
