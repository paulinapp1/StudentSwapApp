using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Application.Interfaces
{
    public interface ImanageUserService
    {
        Task UpdateAddressAsync(int Id, string city, string street, string phoneNumber);
        Task ChangePasswordAsync(int Id, string currentPassword, string newPassword);

    }
}
