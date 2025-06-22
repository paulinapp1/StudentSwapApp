
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Interfaces;

namespace UsersService.Application.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<Microsoft.AspNet.Identity.EntityFramework.IdentityUser> _hasher = new();


        public string Hash(string password)
        {
            return _hasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }


    }
}
