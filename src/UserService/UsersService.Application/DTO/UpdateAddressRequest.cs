using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Application.DTO
{
    public class UpdateAddressRequest
    {
        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string Street { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }


    }
}
