using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application
{
    public class PaymentRequest
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiry { get; set; }
        public string CardCvv { get; set; }
    }
}

