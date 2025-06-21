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
    [Route("listings/[controller]")]
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
        [Authorize(Roles = "Administrator")]
        [HttpDelete("deleteCategory")]
        public async Task<IActionResult> DeleteCategory(int CategoryID)
        {
            var deleted = await _listingRepository.DeleteCategoryAsync(CategoryID);
            if (!deleted)
            {
                return NotFound("Listing with this ID does not exist");

            }
            return Ok(deleted);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPut("updateCategory")]
     
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            if (category == null || category.CategoryId == 0)
            {
                return BadRequest("Invalid category data.");
            }

            var existingCategory = await _listingRepository.GetCategoryByIdAsync(category.CategoryId);
            if (existingCategory == null)
            {
                return NotFound($"Category with ID {category.CategoryId} not found.");
            }

     
            existingCategory.CategoryName = category.CategoryName;
        

            try
            {
                var updatedCategory = await _listingRepository.UpdateCategoryAsync(existingCategory);
                return Ok(updatedCategory);
            }
            catch (Exception)
            {
       
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the category.");
            }
        }

        [Authorize]
        [HttpPost("AddListing")]
        public async Task<IActionResult> AddListing([FromBody] CreateListingRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim == null)
            {
                throw new ArgumentException("Not authorized");
            }
            int userId = int.Parse(userIdClaim.Value);


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
                CategoryId = category.CategoryId,
                status = Status.ACTIVE


            };

            var result = await _addListingService.AddListingAsync(listing);
            return Ok(result);
        }
        [Authorize]
        [HttpDelete("deleteListing")]
        public async Task<IActionResult> DeleteListing(int listingId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var roleClaim = User.FindFirst(ClaimTypes.Role);

            if (userIdClaim == null)
                return Unauthorized("User not authenticated.");

            int userId = int.Parse(userIdClaim.Value);
            string role = roleClaim?.Value ?? "";

            var success = await _addListingService.DeleteListingAsync(listingId, userId, role);

            if (!success)
                return BadRequest("Could not delete listing. Either it does not exist or you're not authorized.");

            return Ok("Listing deleted and removed from carts.");
        }
        


        [Authorize]
        [HttpPatch("updateListingName")]
        public async Task<IActionResult> UpdateListingName(int id, [FromBody] UpdateNameRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name cannot be empty");
            }

            try
            {
                var listing = await _listingRepository.GetByIdAsync(id);
                if (listing == null)
                {
                    return NotFound($"Listing with ID {id} not found");
                }

                listing.Name = request.Name;
                var updatedListing = await _listingRepository.UpdateAsync(listing);

                return Ok(new { message = "Name updated successfully", name = updatedListing.Name });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPatch("updateListingDescription")]
        public async Task<IActionResult> UpdateListingDescription(int id, [FromBody] UpdateDescriptionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return BadRequest("Description cannot be empty");
            }

            try
            {
                var listing = await _listingRepository.GetByIdAsync(id);
                if (listing == null)
                {
                    return NotFound($"Listing with ID {id} not found");
                }

                listing.Description = request.Description;
                var updatedListing = await _listingRepository.UpdateAsync(listing);

                return Ok(new { message = "Description updated successfully", description = updatedListing.Description });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPatch("updateListingPrice")]
        public async Task<IActionResult> UpdateListingPrice(int id, [FromBody] UpdatePriceRequest request)
        {
            if (request.ProductPrice <= 0)
            {
                return BadRequest("Price must be greater than 0");
            }

            try
            {
                var listing = await _listingRepository.GetByIdAsync(id);
                if (listing == null)
                {
                    return NotFound($"Listing with ID {id} not found");
                }

                listing.ProductPrice = request.ProductPrice;
                var updatedListing = await _listingRepository.UpdateAsync(listing);

                return Ok(new { message = "Price updated successfully", productPrice = updatedListing.ProductPrice });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("getById")]
        public async Task<IActionResult> getById([FromQuery] int id)
        {
            var result = await  _listingRepository.GetByIdAsync(id);
            if(result == null)
            {
                return NotFound("Listing with such ID does not exist");
            }
            return Ok(result);
        }
        [HttpGet("getAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _listingRepository.GetAllCategoriesAsync();
            return Ok(result);
        }

        [HttpPatch("updateListingStatus")]
        public async Task<IActionResult> UpdateListingStatus(int listingId, Status newStatus)
        {
            Console.WriteLine(">> [UpdateListingStatus] Endpoint hit.");
            Console.WriteLine($">> ListingId: {listingId}, NewStatus: {newStatus}");

            try
            {
                var updated = await _listingRepository.UpdateStatusAsync(listingId, newStatus);
                Console.WriteLine($">> UpdateStatusAsync returned: {updated}");

                if (!updated)
                {
                    Console.WriteLine(">> Listing not found or update failed.");
                    return NotFound("Listing not found or failed to update status.");
                }

                Console.WriteLine(">> Listing status updated successfully.");
                return Ok("Listing status updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(">> Exception in UpdateListingStatus: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(">> Inner Exception: " + ex.InnerException.Message);
                }

                return StatusCode(500, "Internal error occurred while updating listing.");
            }
        }

        [Authorize]
        [HttpGet("checkStatus")]
        public async Task<IActionResult> CheckStatus(int listingId)
        {
            var result = await _listingRepository.CheckListingStatus(listingId);
            return Ok(result);
        }
    }
}
