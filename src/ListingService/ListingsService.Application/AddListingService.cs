using ListingsService.Application;
using ListingsService.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ListingsService.API.services
{
    public class AddListingService: IAddListingService
    {
        public Listing _listing;
        public HttpContext _httpContext;
        protected IListingRepository _listingRepository;
       
        public AddListingService(Listing listing, HttpContext httpContext, IListingRepository listingRepository)
        {
            _listing = listing;
            _httpContext = httpContext;
            _listingRepository = listingRepository;
        }
        public async Task<Listing> AddListingAsync(Listing listing)
        {
            return await _listingRepository.AddAsync(listing);

        }

        

    }
}
