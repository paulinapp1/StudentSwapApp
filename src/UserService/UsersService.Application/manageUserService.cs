using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Domain.Repositories;

namespace UsersService.Application
{
    public class manageUserService: ImanageUserService
    {
        protected IJwtTokenService _jwtTokenService;
        protected IRepository _repository;
        protected IPasswordHasher _passwordHasher;

        public manageUserService(IJwtTokenService jwtTokenService, IRepository repository, IPasswordHasher passwordHasher)
        {
            _jwtTokenService = jwtTokenService;
            _repository = repository;
            _passwordHasher = passwordHasher;
        }
        public async Task ChangePasswordAsync(int Id, string currentPassword, string newPassword)
        {
            Console.WriteLine($"ChangePasswordAsync started for user ID: {Id}");

            var user = await _repository.GetUserByIdAsync(Id);
            if (user == null)
            {
                Console.WriteLine("User not found in repository");
                throw new Exception("User not found");
            }

            Console.WriteLine($"User found: {user.username}");

            if (!_passwordHasher.VerifyPassword(user.passwordHash, currentPassword))
            {
                Console.WriteLine("Current password verification failed");
                throw new Exception("Current password is incorrect");
            }

            var newHashedPassword = _passwordHasher.Hash(newPassword);
            user.passwordHash = newHashedPassword;

            Console.WriteLine("Password hashed and updated in user object");

            await _repository.UpdateUserAsync(user);

            Console.WriteLine("User updated in repository");
        }

        public async Task UpdateAddressAsync(int Id, string city, string street, string phoneNumber)
        {
            var user = await _repository.GetUserByIdAsync(Id);
            if (user == null)
                throw new Exception("User not found");

            user.City = city;
            user.Street = street;
            user.phone_number = phoneNumber;

            await _repository.UpdateUserAsync(user);
        }

    }
}
