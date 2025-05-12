using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UsersService.Domain.Models;

namespace UsersService.Application
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;

        public JwtTokenService(Microsoft.Extensions.Options.IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
            var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText("./data/private.key")); 
            var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
        }

        public string GenerateToken(int userId, string role)
        {
            
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                        new Claim(ClaimTypes.Role, role)
                    };

        
            

    
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));

         
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

     
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
                signingCredentials: creds
            );

       
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
