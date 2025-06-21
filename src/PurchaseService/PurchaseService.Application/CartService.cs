using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PurchaseService.Application.DTO;
using PurchaseService.Domain;
using PurchaseService.Domain.Enums;
using PurchaseService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Application
{
    public class CartService : ICartService
    {
        protected IPurchaseRepository _repository;
        public CartService(IPurchaseRepository repository)
        {
            _repository = repository;
        }

       
        public async Task<PurchaseModel> AddToCartAsync(int listingId, int userId)
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
                var error = await userResponse.Content.ReadAsStringAsync();
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
                status = Domain.Enums.Status.IN_CART,
                BuyerNumber = int.TryParse(user.PhoneNumber, out var phone) ? phone : 0,
                City = user.City,
                Country = user.Country,
                Street = user.Street,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

      
            return await _repository.AddToCartAsync(purchase);
        }
 
        public async Task<bool> RemoveFromCartAsync(int listingId, int userId)
        {
            var purchase = await _repository.GetCartItemAsync(userId, listingId);
            if (purchase == null || purchase.status != Status.IN_CART) {
                return false; 
            }

            return await _repository.RemoveFromCartAsync(listingId, userId);
        }
        public async Task<List<CartDetailsRequest>> GetCartItems(int userId)
        {
            var items = await _repository.GetCartItemsByUserId(userId);
            var cartDetails = new List<CartDetailsRequest>();
            using var httpClient = new HttpClient();

            foreach (var item in items)
            {
                var response = await httpClient.GetAsync($"http://listingsservice.api:8080/listings/Listing/getById?id={item.ListingId}");
                if (response.IsSuccessStatusCode)
                {
                    var json= await response.Content.ReadAsStringAsync();
                    var listing=JsonConvert.DeserializeObject<CartDetailsRequest>(json);
                    if(listing != null)
                    {
                        cartDetails.Add(listing);
                    }
                }

              
                }
            return cartDetails;
            }
       










    }

}
