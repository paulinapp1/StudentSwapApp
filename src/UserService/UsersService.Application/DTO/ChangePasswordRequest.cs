using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Application.DTO
{
    public class ChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "New password must be at least 8 characters.")]
        public string NewPassword { get; set; }
    }
}
