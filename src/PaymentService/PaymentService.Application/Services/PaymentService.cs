using PaymentService.Application.Interfaces;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Interfaces;
using PaymentService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ICreditCardService _cardService;
        private readonly ITransactionRepository _repository;
        public PaymentService(ICreditCardService cardService, ITransactionRepository repository)
        {
            _cardService = cardService;
            _repository = repository;
        }

        public async Task<PaymentResult> ProcessPayment(PaymentRequest request)
        {
            _cardService.ValidateCard(request.CardNumber);
            var provider = _cardService.GetCardType(request.CardNumber);

            var result = new PaymentResult
            {
                Success = true,
                Message = "Payment approved.",
                Status = PaymentStatus.Completed
            };

            var transaction = new PaymentTransaction
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                UserId = request.UserId,
                Amount = request.Amount,
                CardProvider = provider,
                CardLast4 = request.CardNumber[^4..],
                Status = result.Success ? PaymentStatus.Completed : PaymentStatus.Failed,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(transaction);

            return new PaymentResult
            {
                Success = true,
                Message = "Payment validated, transaction created.",
                Status = PaymentStatus.Pending
            };
        }
    }
}

