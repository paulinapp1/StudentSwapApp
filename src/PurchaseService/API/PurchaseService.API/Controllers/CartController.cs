using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseService.Application;
using PurchaseService.Domain;
using PurchaseService.Domain.Repositories;
using System.Security.Claims;

namespace PurchaseService.API.Controllers
{
    [Route("purchase/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IPurchaseRepository _repository;

        public CartController(ICartService cartService, IPurchaseRepository repository)
        {
            _cartService = cartService;
            _repository = repository;
        }
        [Authorize]
        [HttpPost("addToCart")]
        public async Task<IActionResult> AddToCart([FromQuery]int listingId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
         

            int userId = int.Parse(userIdClaim.Value);
            bool alreadyInCart = await _repository.IsItemInCartAsync(listingId, userId);
            if (alreadyInCart)
                return Conflict("Item is already in your cart.");

            var result = await _cartService.AddToCartAsync(listingId, userId);
            return Ok(result);
        }
     
        [HttpDelete("removeFromCart")]
        public async Task<IActionResult> RemoveFromCart([FromQuery]int  listingId)
        {
            var userIdClaim=User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);

            var result= await _cartService.RemoveFromCartAsync(listingId, userId);
            if (!result)
            {
                return NotFound("Item is not in cart");
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("getCartItems")]
        public async Task<IActionResult> GetCartItems()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);
            var result = await _cartService.GetCartItems(userId);
            if (result.Count==0)
            {
                return NotFound("No items in cart");
            }
            return Ok(result);
        }


        [HttpDelete("removeListingFromCarts")]
        public async Task<IActionResult> RemoveListingFromCarts([FromQuery] int listingId)
        {
            await _repository.RemoveListingFromAllCartsAsync(listingId);
            return Ok();
        }

    }
}

