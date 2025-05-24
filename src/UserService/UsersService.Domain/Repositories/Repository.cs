using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Domain.Models;

namespace UsersService.Domain.Repositories
{
    public class Repository : IRepository
    {
        private readonly DataContext _dataContext;
        public Repository(DataContext dataContext) {
            _dataContext = dataContext;
        }
        public async Task<UserModel> AddUserAsync(UserModel user)
        {
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }
        

        public async Task<bool> EmailAlreadyExistsAsync(string email)
        {
                return await _dataContext.Users.AnyAsync(u => u.email == email);
        }

        public async Task<UserModel> GetUserAsync(string username) //zmienic na username
        {
            return await _dataContext.Users.Where(x => x.username == username).FirstOrDefaultAsync();

        }



        public async Task<bool> UserAlreadyExistsAsync(string username)
        {
           return await _dataContext.Users.AnyAsync(u => u.username == username);
        }
        public async Task<UserModel> UpdateUserAsync(UserModel user)
        {
            var existingUser = await _dataContext.Users.FindAsync(user.username);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {user.Id} not found.");
            }

            // Update properties (example, update all relevant fields)
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.City = user.City;
            existingUser.Country = user.Country;
            existingUser.Street = user.Street;
            existingUser.phone_number = user.phone_number;
            existingUser.email = user.email;
            existingUser.passwordHash = user.passwordHash;
            existingUser.Role = user.Role;
            existingUser.username = user.username;

            _dataContext.Users.Update(existingUser);
            await _dataContext.SaveChangesAsync();
            return existingUser;
        }

    
        public async Task<bool> DeleteUserAsync(string username)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.username == username);
            if (user == null)
            {
                return false; 
            }

            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();
            return true;
        }

    }
}
