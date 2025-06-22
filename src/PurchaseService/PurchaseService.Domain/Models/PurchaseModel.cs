using PurchaseService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Domain.Models
{
    public class PurchaseModel
    {

        public int PurchaseId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int ListingId { get; set; }
        public decimal Price { get; set; }
        public int BuyerNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Status status { get; set; }

    }
}
