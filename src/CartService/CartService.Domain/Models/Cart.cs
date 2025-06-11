
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Domain.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public List<Listing> Listings { get; set; } = new List<Listing>();

    }
}
