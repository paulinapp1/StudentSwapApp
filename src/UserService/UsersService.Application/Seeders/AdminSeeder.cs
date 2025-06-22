
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Interfaces;
using UsersService.Domain.Models;
using UsersService.Domain.Repositories;



namespace UsersService.Domain.Seeders
{
    public class AdminSeeder
    {
        private readonly IRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        public AdminSeeder(IRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }
        public async Task SeedAsync()
        {
            const string adminUsername = "admin";
            const string adminEmail = "admin@lol.pl";
            const string adminPassword = "lubiepierogi123";

            var adminExists = await _repository.UserAlreadyExistsAsync(adminUsername);
            if (!adminExists)
            {
                var adminUser = new UserModel
                {
                   
            
                FirstName = "Admin",
                LastName = "User",
                City = "AdminCity",
                Street = "AdminStreet",
                Country = "AdminCountry",
                phone_number = "0000000000",
                username = "admin",
                email = "admin@example.com",
                Role = "Administrator",
                passwordHash = _passwordHasher.Hash("LubiePIEROGI123") 
            


                };
                await _repository.AddUserAsync(adminUser);

            }
        }
    }
}
