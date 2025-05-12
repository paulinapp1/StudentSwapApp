using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Application.Exceptions
{
    
    
        public class UserAlreadyExistsException : Exception
        {
            public UserAlreadyExistsException() : base("Incorect password or login") { }
        }
    }

