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
    }
}
