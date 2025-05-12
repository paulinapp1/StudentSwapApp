using Microsoft.AspNetCore.Http;
using StudentSwapApp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.Exceptions;
using UsersService.Domain.Models;
using UsersService.Domain.Repositories;


namespace UsersService.Application
{
    public class LoginService : ILoginService
    {
        protected IJwtTokenService _jwtTokenService;
        protected IRepository _repository;
        protected IPasswordHasher _passwordHasher;

        public LoginService( IJwtTokenService jwtTokenService, IRepository repository, IPasswordHasher passwordHasher)
        {
            _jwtTokenService = jwtTokenService;
            _repository = repository;
            _passwordHasher = passwordHasher;
        }
        private bool IsValidEmail(string email)
        {
            throw new NotImplementedException();
        }
        public async Task<AuthResponse> SignUp(SignUpRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName)) throw new ArgumentException("First name is required");
            if (string.IsNullOrWhiteSpace(request.LastName)) throw new ArgumentException("Last name is required");
            if (string.IsNullOrWhiteSpace(request.username))
                throw new ArgumentException("Username is required.");
            if (string.IsNullOrWhiteSpace(request.email)) throw new ArgumentException("Email is required");
            if (string.IsNullOrWhiteSpace(request.password) || request.password.Length < 8) throw new ArgumentException("to short password");
            if (await _repository.UserAlreadyExistsAsync(request.username)) throw new ArgumentException("this username is taken :( ");
            if (await _repository.EmailAlreadyExistsAsync(request.email)) throw new ArgumentException("there already exists an account with this email address");

            string passwordHash = _passwordHasher.Hash(request.password);

            var newUser = new UserModel
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                City = request.City,
                Street = request.Street,
                Country = request.Country,
                phone_number = request.phone_number,
                username = request.username,
                passwordHash = passwordHash,
                Role = "User"
            };
            await _repository.AddUserAsync(newUser);

            var token = _jwtTokenService.GenerateToken(newUser.Id, newUser.Role);

            return new AuthResponse
            {
                Token = token,
                UserId = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                username = newUser.username,
            };


        }
        public async Task<string> Login(string username, string password)
        {
            
            var user = await _repository.GetUserAsync(username);

     
            if (user == null)
            {
                throw new InvalidCredentialsException();
            }

     
            var passwordValid = _passwordHasher.VerifyPassword(password, user.passwordHash);
            if (!passwordValid)
            {
                throw new InvalidCredentialsException();
            }


            string role = user.Role;

            var token = _jwtTokenService.GenerateToken(user.Id, role);
            
            return token;
        }



    }
}
