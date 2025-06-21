using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Application.DTO
{
    public class CreatePurchaseRequest
    {
        public int ListingId { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiry { get; set; } 
        public string CardCvv { get; set; }
    }
}
