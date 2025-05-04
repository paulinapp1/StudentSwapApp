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
        Task<UserModel> GetUserAsync (int id);
        Task<List<UserModel>> GetUsersAsync();
        Task<UserModel> AddUserAsync(UserModel user);
        Task<UserModel> UpdateUserAsync(UserModel user);
    }
}
