using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersService.Application.DTO;
using UsersService.Application.Exceptions;
using UsersService.Application.Interfaces;

namespace StudentSwapApp.API.Controllers
{
    [Route("users/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        protected ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
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
                    ex.Message
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ex.Message
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
        [HttpPost("signUpAdmin")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SignUpAdmin([FromBody] SignUpRequest signUpRequest)
        {

            try
            {
                var authResponse = await _loginService.SignUpAdmin(signUpRequest);

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
                    ex.Message
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    ex.Message
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
                var token = await _loginService.Login(request.Username, request.Password);
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


    }
}
