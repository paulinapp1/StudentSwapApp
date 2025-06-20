using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Exceptions
{
    public class CreditCardNumberTooShortException : Exception
    {
        public CreditCardNumberTooShortException() : base("Card number is too short") { }
        public CreditCardNumberTooShortException(string message) : base(message) { }
        public CreditCardNumberTooShortException(string message, Exception innerException) : base(message, innerException) { }
    }
}
