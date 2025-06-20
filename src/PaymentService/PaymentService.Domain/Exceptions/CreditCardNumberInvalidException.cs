using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Exeptions
{
    public class CreditCardNumberInvalidException : Exception
    {
        public CreditCardNumberInvalidException() : base("Card number is invalid") { }
        public CreditCardNumberInvalidException(string message) : base(message) { }
        public CreditCardNumberInvalidException(string message, Exception innerException) : base(message, innerException) { }
    }
}

