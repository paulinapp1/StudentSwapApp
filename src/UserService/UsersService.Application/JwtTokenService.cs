using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UsersService.Domain.Models;

namespace UsersService.Application
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;
        private readonly RsaSecurityKey _rsaKey;
        private readonly SigningCredentials _signingCredentials;

        public JwtTokenService(Microsoft.Extensions.Options.IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;

            // Load RSA private key from PEM file
            var rsa = RSA.Create();
            rsa.ImportFromPem(System.IO.File.ReadAllText("./data/private.key"));

            _rsaKey = new RsaSecurityKey(rsa);

            // Create signing credentials using RSA SHA256
            _signingCredentials = new SigningCredentials(_rsaKey, SecurityAlgorithms.RsaSha256);
        }

        public string GenerateToken(int userId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
                signingCredentials: _signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
