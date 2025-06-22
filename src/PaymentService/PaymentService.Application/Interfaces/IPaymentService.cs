using PaymentService.Application.DTO;
using PaymentService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentService
    {
        public Task<PaymentResult> ProcessPayment(PaymentRequest request);
    }
}

