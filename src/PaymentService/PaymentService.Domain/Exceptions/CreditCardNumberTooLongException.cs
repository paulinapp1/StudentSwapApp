using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Exceptions
{
    public class CreditCardNumberTooLongException : Exception
    {
        public CreditCardNumberTooLongException() : base("Card number is too long") { }
        public CreditCardNumberTooLongException(string message) : base(message) { }
        public CreditCardNumberTooLongException(string message, Exception innerException) : base(message, innerException) { }
    }
}
