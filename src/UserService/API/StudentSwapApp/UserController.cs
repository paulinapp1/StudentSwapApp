using Microsoft.AspNetCore.Mvc;
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
