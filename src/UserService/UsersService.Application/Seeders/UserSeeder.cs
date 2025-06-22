using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Interfaces;
using UsersService.Domain.Models;
using UsersService.Domain.Repositories;

namespace UsersService.Application.Seeders
{
    public class UserSeeder
    {
        private readonly IRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public UserSeeder(IRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync()
        {
            
            await SeedUserIfNotExists("paula1234", "paula@lol.pl", "skubidubi123",
                "Paulina", "Skubi", "PaulinaCity", "PaulinaStreet", "PaulinaCountry", "0000000000");

            
            await SeedUserIfNotExists("supersylwia", "sylwia@lol.pl", "sylwiaiapollo123",
                "Sylwia", "Super", "SylwiaCity", "SylwiaStreet", "SylwiaCountry", "1234567890");
        }

        private async Task SeedUserIfNotExists(string username, string email, string password,
            string firstName, string lastName, string city, string street, string country, string phone)
        {
            var userExists = await _repository.UserAlreadyExistsAsync(username);
            if (!userExists)
            {
                var user = new UserModel
                {
                    FirstName = firstName,
                    LastName = lastName,
                    City = city,
                    Street = street,
                    Country = country,
                    phone_number = phone,
                    username = username,
                    email = email,
                    Role = "User",
                    passwordHash = _passwordHasher.Hash(password)
                };
                await _repository.AddUserAsync(user);
            }
        }
    }
}
