using ListingsService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListingsService.Domain.Models
{
    public class Listing
    {
        public int ListingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal ProductPrice { get; set; }
        public Condition Condition { get; set; }
        public int UserId { get; set; }
        public Status status { get; set; }

        public Category Category { get; set; }

    }
}
