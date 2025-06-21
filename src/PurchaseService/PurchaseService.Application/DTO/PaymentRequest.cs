using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Application.DTO
{
    public class PaymentRequest
    {
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiry { get; set; }
        public string CardCvv { get; set; }
        public int ListingId { get; set; }
    }
}
