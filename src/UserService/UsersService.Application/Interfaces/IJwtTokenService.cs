using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(int userId, string role);
    }
}
