using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Http;
using System.Text;
using Microsoft.AspNetCore.Http;


namespace UsersService.Application
{
    public class FirebaseAuthService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor; 

        public FirebaseAuthService(IConfiguration config, System.Net.Http.IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _apiKey = config["Firebase:WebApiKey"];
            _httpClient = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor; 
            if (FirebaseApp.DefaultInstance == null)
            {
                var firebaseKeyPath = config["Firebase:KeyPath"];
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(firebaseKeyPath)
                });
            }
        }

        public async Task<string> VerifyIdTokenAsync(string idToken)
        {
            try
            {
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                return decodedToken.Uid;
            }
            catch
            {
                return null;
            }
        }
        private void SetAuthCookies(string idToken, string RefreshToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return;
            }
            var decodedToken = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken).Result;
            var uid = decodedToken.Uid;

            httpContext.Response.Cookies.Append("authToken", idToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.Now.AddHours(1)
            });
            httpContext.Response.Cookies.Append("userUID", uid, new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.Now.AddHours(1),
                
            });


        }
        public void ClearAuthCookies()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;
            httpContext.Response.Cookies.Delete("authToken");
            httpContext.Response.Cookies.Delete("userUID");
   
        }
        public async Task<string?> SignInWithEmailPasswordAsync(string email, string password)
        {
            var payload = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_apiKey}",
                content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthResponse>(responseBody);
            if(result?.IdToken != null)
            {
                SetAuthCookies(result.IdToken, result.RefreshToken);
            }
            return result?.IdToken;
        }
    }

   
}
