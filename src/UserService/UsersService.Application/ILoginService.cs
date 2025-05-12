using StudentSwapApp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Application
{
    public interface ILoginService
    {
        Task<string> Login(string username, string password);
        Task<AuthResponse> SignUp(SignUpRequest request);
    }
}
