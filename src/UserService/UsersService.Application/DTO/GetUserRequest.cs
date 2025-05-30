using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Application.DTO
{
    internal class GetUserRequest
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string Street { get; set; }

        [MaxLength(100)]
        public string Country { get; set; }

        [MaxLength(20)]
        public string phone_number { get; set; }
        [Required]
        [MaxLength(100)]
        public string username { get; set; }
        [Required]
        [MaxLength(100)]
        public string email { get; set; }

    }
}
