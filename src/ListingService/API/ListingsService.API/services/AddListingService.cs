using ListingsService.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ListingsService.API.services
{
    public class AddListingService
    {
        public Listing _listing;
        public HttpContext _httpContext;

        public AddListingService( Listing listing, HttpContext httpContext)
        {
            _listing = listing;
            _httpContext = httpContext;
        }

        public int? GetUserIdFromToken(HttpContext httpContext)
        {
            var token = httpContext.Request.Cookies["AuthToken"];
            if(token == null)
            {
                return null;
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            return userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
        }

    }
}
