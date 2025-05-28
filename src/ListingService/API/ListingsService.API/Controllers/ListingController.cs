using ListingsService.API.services;
using ListingsService.Domain.Enums;
using ListingsService.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ListingsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IListingRepository _listingRepository;
        private readonly AddListingService _addListingService;
        public ListingController(IListingRepository listingRepository, AddListingService addListingService)
        {
            _listingRepository = listingRepository;
            _addListingService = addListingService;
        }

        [HttpGet("getAllListings")]
        public async Task<ActionResult> GetAllListings()
        {
            var result = await  _listingRepository.GetAllAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddListing([FromBody] Listing request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            int userId = int.Parse(userIdClaim.Value); // or use TryParse for safety

            // Create the Listing object
            var listing = new Listing
            {
           
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                ProductPrice = request.ProductPrice,
                Condition= request.Condition,
                UserId = userId
               ,
            };

            var result = await _addListingService.AddListingAsync(listing);
            return Ok(result);
        }
    }
}
