using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Domain.Models;

namespace UsersService.Domain.Repositories
{
    public interface IRepository
    {
        Task<UserModel> GetUserAsync (string username);
        
        Task<UserModel> AddUserAsync(UserModel user);

        Task<bool> UserAlreadyExistsAsync(string username);
        Task<bool> EmailAlreadyExistsAsync(string email);
        Task<UserModel> UpdateUserAsync(UserModel user);
        Task<bool> DeleteUserAsync(string username);



    }
}
