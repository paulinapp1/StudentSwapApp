using Newtonsoft.Json;
using PurchaseService.Application.DTO;
using PurchaseService.Domain;
using PurchaseService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Application
{
    public class CreatePurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;
        public CreatePurchaseService(IPurchaseRepository repository)
        {
            _repository = repository;
        }
        public async Task<PurchaseModel> CancelPurchase(int purchaseId)
        {
            // Retrieve the purchase from the database
            var purchase = await _repository.GetByIdAsync(purchaseId);
            if (purchase == null)
            {
                throw new Exception($"Purchase with ID {purchaseId} not found.");
            }

            int listingId = purchase.ListingId;

            // Update listing status
            using var httpClient = new HttpClient();
            var updateStatusUrl = $"http://listingsservice.api:8080/listings/Listing/updateListingStatus?listingId={listingId}&status=AVAILABLE";
            var updateResponse = await httpClient.PutAsync(updateStatusUrl, null);

            if (!updateResponse.IsSuccessStatusCode)
            {
                var error = await updateResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to update listing status for ListingId: {listingId}");
                // Optionally: throw an exception or return an error result
            }

           
            purchase.status = Domain.Enums.Status.CANCELLED;
            purchase.UpdatedAt = DateTime.UtcNow;

            await _repository.CancelPurchase(purchaseId);

            return purchase;
        }

        public async Task<PurchaseModel> createPurchase(int listingId, int userId)
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
            var updateStatusUrl = $"http://listingsservice.api:8080/listings/Listing/updateListingStatus?listingId={listingId}&status=RESERVED";
            var updateResponse = await httpClient.PutAsync(updateStatusUrl, null);

            if (!updateResponse.IsSuccessStatusCode)
            {
                
                Console.WriteLine($"Failed to update listing status for ListingId");
              

            }

            //dodac call do payment service
            return await _repository.CreatePurchaseAsync(purchase);

        }
    }
}
