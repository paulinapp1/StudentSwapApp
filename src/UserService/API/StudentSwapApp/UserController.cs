using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersService.Domain.Models;
using UsersService.Domain.Repositories;

namespace StudentSwapApp.API
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IRepository _repository;
        private readonly HttpClient _httpClient;

        public UserController(IRepository repository, HttpClient httpClient)
        {
            _repository = repository;
            _httpClient = httpClient;
        }
        [HttpPut("{username}")]
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var deleted = await _repository.DeleteUserAsync(username);

            if (!deleted)
            {
                return NotFound($"User with username '{username}' not found.");
            }

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
