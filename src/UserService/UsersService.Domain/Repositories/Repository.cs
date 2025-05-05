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

        public async Task<UserModel> GetUserAsync(string id)
        {
            return await _dataContext.Users.Where(x => x.UID == id).FirstOrDefaultAsync();

        }

        
    }
}
