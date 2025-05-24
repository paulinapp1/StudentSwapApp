using ListingsService.API.services;
using ListingsService.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    }
}
