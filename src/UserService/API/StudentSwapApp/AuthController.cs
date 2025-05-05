using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UsersService.Application;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using UsersService.Domain.Models;
using UsersService.Domain.Repositories;

namespace StudentSwapApp.API
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        public readonly HttpClient _httpClient;
        private readonly FirebaseAuthService _firebaseAuthService;
        private readonly IRepository _repository;

        public AuthController(IConfiguration config, FirebaseAuthService firebaseAuthService,
             IRepository repository)
        {
            _firebaseAuthService = firebaseAuthService;
            _config = config;
            _httpClient = new HttpClient();
            _repository = repository;
        }
        private async Task<string> MakeApiCallWithToken(string idToken)
        {
            var apiKey = _config["Firebase:WebApiKey"];

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://identitytoolkit.googleapis.com/v1/accounts:lookup?key={apiKey}")
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(new { idToken }),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new Exception("API request failed: " + await response.Content.ReadAsStringAsync());
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] AuthRequest request)
        {
            var apiKey = _config["Firebase:WebApiKey"]; 

            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("Klucza nie ma :( ");
            }
            var payload = new
            {
                email = request.Email,
                password = request.Password,
                returnSecureToken = true
            };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}", content);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return BadRequest(body);

            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(body);
            var newUser = new UserModel
            {
                UID = authResponse.LocalId,
                FirstName = request.Name,
                LastName = request.Surname,
                City = request.City,
                Street = request.street,
                Country = request.Country,
                phone_number = request.Phone_number,
                email = request.Email,
                username = request.Email 
            };

            await _repository.AddUserAsync(newUser); 

            return Ok(authResponse);

        }
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            
            var idToken = await _firebaseAuthService.SignInWithEmailPasswordAsync(request.Email, request.Password);
            
        
            var apiResponse = await MakeApiCallWithToken(idToken);

            return Ok(apiResponse);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _firebaseAuthService.ClearAuthCookies();
            return Ok(new {Message = "Wylogowano"});
        }
        

    }
}
