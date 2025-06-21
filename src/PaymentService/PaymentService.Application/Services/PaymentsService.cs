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
    public class PaymentsService : IPaymentService
    {
        private readonly ICreditCardService _cardService;
        private readonly ITransactionRepository _repository;
        public PaymentsService(ICreditCardService cardService, ITransactionRepository repository)
        {
            _cardService = cardService;
            _repository = repository;
        }

      
        public async Task<PaymentResult> ProcessPayment(PaymentRequest request)
        {
            using var httpClient = new HttpClient();
            _cardService.ValidateCard(request.CardNumber);
            var provider = _cardService.GetCardType(request.CardNumber);

            // Tworzymy transakcję jako PENDING
            var transaction = new PaymentTransaction
            {
                PurchaseId = request.PurchaseId,
                UserId = request.UserId,
                Amount = request.Amount,
                CardProvider = provider,
                CardLast4 = request.CardNumber[^4..],
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(transaction);

            // Usuwanie z koszyków
           // var deleteResponse = await httpClient.DeleteAsync(
             //   $"http://purchaseservice.api:8080/purchase/Cart/removeListingFromCarts?listingId={request.ListingId}");

           // if (!deleteResponse.IsSuccessStatusCode)
          //  {
           //     throw new Exception($"Cart removal failed: {deleteResponse.StatusCode}");
          //  }

            // Aktualizacja statusu ogłoszenia
            var updateStatusUrl =
                $"http://listingsservice.api:8080/listings/Listing/updateListingStatus?listingId={request.ListingId}&status=SOLD";

            var updateResponse = await httpClient.PatchAsync(updateStatusUrl, null);

            if (!updateResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Listing update failed: {updateResponse.StatusCode}");
            }

           

            return new PaymentResult
            {
                Success = true,
                Message = "Payment processed and listing updated.",
                Status = PaymentStatus.Completed
            };
        }


    }
}

