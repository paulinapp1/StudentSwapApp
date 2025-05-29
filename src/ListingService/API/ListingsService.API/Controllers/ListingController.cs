using ListingsService.API.DTO;
using ListingsService.Application;
using ListingsService.Domain.Enums;
using ListingsService.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UsersService.Domain;

namespace ListingsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IListingRepository _listingRepository;
        private readonly IAddListingService _addListingService;
       
        public ListingController(IListingRepository listingRepository, IAddListingService addListingService)
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
        [Authorize(Roles = "Administrator")]
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request)
        {
            var category = new Category
            {
                CategoryName = request.CategoryName,
            };
            var result =await _listingRepository.AddCategoryAsync(category);
            return Ok(result);
          
        }

        [Authorize]
        [HttpPost("AddListing")]
        public async Task<IActionResult> AddListing([FromBody] CreateListingRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return BadRequest("Invalid user ID in token.");


            var category = await _listingRepository.GetByNameAsync(request.CategoryName);

            if (category == null)
                return BadRequest($"Category '{request.CategoryName}' not found.");

          
            var listing = new Listing
            {
                Name = request.Name,
                Description = request.Description,
                ProductPrice = request.ProductPrice,
                Condition = request.Condition,
                UserId = userId,
                CategoryId = category.CategoryId


            };

            var result = await _addListingService.AddListingAsync(listing);
            return Ok(result);
        }

    }
}
