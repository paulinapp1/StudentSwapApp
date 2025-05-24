using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Application;
using UsersService.Application.DTO;
using UsersService.Application.Exceptions;

namespace StudentSwapApp.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        protected ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService=loginService;
        }
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest signUpRequest)
        {
            
            try
            {
                var authResponse = await _loginService.SignUp(signUpRequest);

                return Ok(new
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = authResponse
                });
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred during registration"
                });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await  _loginService.Login(request.Username, request.Password);
                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                });
                return Ok(new { token });
            }
            catch (InvalidCredentialsException)
            {
                return Unauthorized();
            }
        }
        [HttpGet("authorizeadmin")]
        [Authorize]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminPage()
        {
            return Ok("Dane tylko dla administratora");
        }

    }
}
