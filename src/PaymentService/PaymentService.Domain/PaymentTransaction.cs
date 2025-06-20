using PaymentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain
{
    public class PaymentTransaction
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public CreditCardProvider CardProvider { get; set; }
        public string CardLast4 { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}

