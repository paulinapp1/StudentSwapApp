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

        public async Task<UserModel> GetUserAsync(string username)
        {
            return await _dataContext.Users.Where(x => x.username == username).FirstOrDefaultAsync();

        }
        public async Task<UserModel> GetUserByIdAsync(int Id)
        {
            var user = await _dataContext.Users.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.passwordHash = null; 
            }
            return user;

        }




        public async Task<bool> UserAlreadyExistsAsync(string username)
        {
           return await _dataContext.Users.AnyAsync(u => u.username == username);
        }
        public async Task<UserModel> UpdateUserAsync(UserModel user)
        {
            var existingUser = await _dataContext.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {user.Id} not found.");
            }

            if (!string.IsNullOrEmpty(user.FirstName))
                existingUser.FirstName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
                existingUser.LastName = user.LastName;
            if (!string.IsNullOrEmpty(user.City))
                existingUser.City = user.City;
            if (!string.IsNullOrEmpty(user.Country))
                existingUser.Country = user.Country;
            if (!string.IsNullOrEmpty(user.Street))
                existingUser.Street = user.Street;
            if (!string.IsNullOrEmpty(user.phone_number))
                existingUser.phone_number = user.phone_number;
            if (!string.IsNullOrEmpty(user.email))
                existingUser.email = user.email;
            if (!string.IsNullOrEmpty(user.passwordHash))
                existingUser.passwordHash = user.passwordHash;
            if (!string.IsNullOrEmpty(user.Role))
                existingUser.Role = user.Role;
            if (!string.IsNullOrEmpty(user.username))
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
