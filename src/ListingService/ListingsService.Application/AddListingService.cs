using ListingsService.Application;
using ListingsService.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ListingsService.Application
{
    public class AddListingService: IAddListingService
    {
        
        public IHttpContextAccessor _httpContextAccessor;

        protected IListingRepository _listingRepository;
       
        public AddListingService(IHttpContextAccessor httpContextAccessor, IListingRepository listingRepository)
        {

            _httpContextAccessor = httpContextAccessor;
            _listingRepository = listingRepository;
        }
        public async Task<Listing> AddListingAsync(Listing listing)
        {
            return await _listingRepository.AddAsync(listing);

        }

        

    }
}
