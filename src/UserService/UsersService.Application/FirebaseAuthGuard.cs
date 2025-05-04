using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using UsersService.Application;

public class FirebaseAuthGuard
{
    private readonly RequestDelegate _next;

    public FirebaseAuthGuard(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();

        if (!string.IsNullOrEmpty(token))
        {
     
            var authService = context.RequestServices.GetRequiredService<FirebaseAuthService>();

            var uid = await authService.VerifyIdTokenAsync(token);
            if (uid != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, uid)
                };
                var identity = new ClaimsIdentity(claims, "firebase");
                context.User = new ClaimsPrincipal(identity);
            }
        }

        await _next(context);
    }
}
