using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersService.Application.DTO;

namespace UsersService.Application
{
    public interface ILoginService
    {
        Task<string> Login(string username, string password);
        Task<AuthResponse> SignUp(SignUpRequest request);
        Task<AuthResponse> SignUpAdmin(SignUpRequest request);
    }
}
