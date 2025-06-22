using ListingsService.Application.Interfaces;
using ListingsService.Domain.Models;
using ListingsService.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace ListingsService.Application.Services
{
    public class AddListingService : IAddListingService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IListingRepository _listingRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public AddListingService(
            IHttpContextAccessor httpContextAccessor,
            IListingRepository listingRepository,
            IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _listingRepository = listingRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Listing> AddListingAsync(Listing listing)
        {
            return await _listingRepository.AddAsync(listing);
        }

        public async Task<bool> DeleteListingAsync(int listingId, int userId, string role)
        {
            var listing = await _listingRepository.GetByIdAsync(listingId);
            if (listing == null)
                return false;

            if (listing.UserId != userId && role != "Admin")
                return false;

            var deleted = await _listingRepository.DeleteAsync(listingId);
            if (!deleted)
                return false;

            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync($"http://purchaseservice.api:8080/purchase/Cart/removeListingFromCarts?listingId={listingId}");

            return true;
        }
    }

}




