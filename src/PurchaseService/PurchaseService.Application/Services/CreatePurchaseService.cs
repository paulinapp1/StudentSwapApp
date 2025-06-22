using Newtonsoft.Json;
using PurchaseService.Application.DTO;
using PurchaseService.Application.Interfaces;
using PurchaseService.Domain.Models;
using PurchaseService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Application.Services
{
    public class CreatePurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;
        private readonly ICartService _cartService;
        public CreatePurchaseService(IPurchaseRepository repository, ICartService cartService)
        {
            _repository = repository;
            _cartService = cartService;
        }
        public async Task<PurchaseModel> CancelPurchase(int purchaseId)
        {

            var purchase = await _repository.GetByIdAsync(purchaseId);
            if (purchase == null)
            {
                throw new Exception($"Purchase with ID {purchaseId} not found.");
            }

            int listingId = purchase.ListingId;




            using var httpClient = new HttpClient();
            var updateStatusUrl = $"http://listingsservice.api:8080/listings/Listing/updateListingStatus?listingId={listingId}&newStatus=1";
            var updateResponse = await httpClient.PutAsync(updateStatusUrl, null);

            if (!updateResponse.IsSuccessStatusCode)
            {
                var error = await updateResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to update listing status for ListingId: {listingId}");

            }



            purchase.status = Domain.Enums.Status.CANCELLED;
            purchase.UpdatedAt = DateTime.UtcNow;

            await _repository.CancelPurchase(purchaseId);

            return purchase;
        }

        public async Task<PurchaseModel> CreatePurchase(int listingId, int userId, CreatePurchaseRequest paymentRequest)
        {
            using var httpClient = new HttpClient();
            var listingResponse = await httpClient.GetAsync($"http://listingsservice.api:8080/listings/Listing/getById?id={listingId}");
            if (!listingResponse.IsSuccessStatusCode)
            {

                throw new Exception($"Failed to retrieve listing ");
            }

            var listingJson = await listingResponse.Content.ReadAsStringAsync();
            var listing = JsonConvert.DeserializeObject<ListingRequest>(listingJson);
            if (listing == null)
                throw new Exception("Listing data is null or invalid");
            var currentStatus = listing.Status;
            var listingPrice = listing.Price;
            if (currentStatus == 3 || currentStatus == 2)
            {
                throw new Exception("This listing has been sold  or it's reserved :( ");
            }

            var userResponse = await httpClient.GetAsync($"http://studentswapapp.api:8080/users/User/getUserById?id={userId}");
            if (!userResponse.IsSuccessStatusCode)
            {

                throw new Exception($"Failed to retrieve user");
            }

            var userJson = await userResponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserRequest>(userJson);
            if (user == null)
                throw new Exception("User data is null or invalid");


            var purchase = new PurchaseModel
            {
                ListingId = listingId,
                BuyerId = userId,
                SellerId = listing.UserId,
                Price = listing.Price,
                status = Domain.Enums.Status.CREATED,
                BuyerNumber = int.TryParse(user.PhoneNumber, out var phone) ? phone : 0,
                City = user.City,
                Country = user.Country,
                Street = user.Street,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };


            var createdPurchase = await _repository.CreatePurchaseAsync(purchase);
            if (listingPrice != 0)
            {
                var paymentData = new PaymentRequest
                {
                    PurchaseId = createdPurchase.PurchaseId,
                    UserId = createdPurchase.BuyerId,
                    Amount = listing.Price,
                    ListingId = listingId,
                    CardNumber = paymentRequest.CardNumber,
                    CardExpiry = paymentRequest.CardExpiry,
                    CardCvv = paymentRequest.CardCvv

                };
                var paymentJson = JsonConvert.SerializeObject(paymentData);
                var paymentContent = new StringContent(paymentJson, Encoding.UTF8, "application/json");
                try
                {
                    var paymentResponse = await httpClient.PostAsync("http://paymentservice.api:8080/api/Payments", paymentContent);
                    if (paymentResponse.IsSuccessStatusCode)
                    {
                        var paymentResultJson = await paymentResponse.Content.ReadAsStringAsync();
                        var paymentResult = JsonConvert.DeserializeObject<PaymentResult>(paymentResultJson);
                        if (paymentResult?.Success == true)
                        {
                            var updatePaymentStatusUrl = $"http://listingsservice.api:8080/listings/Listing/updateListingStatus?listingId={listingId}&newStatus=3";
                            var updatePaymentResponse = await httpClient.PatchAsync(updatePaymentStatusUrl, null);
                            if (!updatePaymentResponse.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"Failed to update listing status for ListingId: {listingId}");
                            }

                            createdPurchase.status = Domain.Enums.Status.COMPLETED;
                            createdPurchase.UpdatedAt = DateTime.UtcNow;

                            await _repository.UpdatePurchaseAsync(createdPurchase);

                            return createdPurchase;
                        }
                        else
                        {
                            createdPurchase.status = Domain.Enums.Status.PAYMENT_PENDING;
                            await _repository.UpdatePurchaseAsync(createdPurchase);
                            throw new Exception($"Payment failed");
                        }
                    }
                    else
                    {
                        createdPurchase.status = Domain.Enums.Status.PAYMENT_PENDING;
                        await _repository.UpdatePurchaseAsync(createdPurchase);
                        var errorContent = await paymentResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Payment service call failed. Status: {paymentResponse.StatusCode}, Body: {errorContent}");

                        createdPurchase.status = Domain.Enums.Status.PAYMENT_PENDING;
                        await _repository.UpdatePurchaseAsync(createdPurchase);
                        throw new Exception($"Payment service call failed: {paymentResponse.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    createdPurchase.status = Domain.Enums.Status.PAYMENT_PENDING;
                    await _repository.UpdatePurchaseAsync(createdPurchase);
                    throw new Exception($"Payment processing error: {ex.Message}");
                }


            }
            else
            {
                return createdPurchase;
            }
        }
    }
}