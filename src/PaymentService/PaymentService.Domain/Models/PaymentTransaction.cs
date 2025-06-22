using PaymentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Models
{
    public class PaymentTransaction
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public CreditCardProvider CardProvider { get; set; }
        public string CardLast4 { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}

