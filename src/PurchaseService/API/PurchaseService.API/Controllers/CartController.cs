using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PurchaseService.Application;
using PurchaseService.Domain;
using System.Security.Claims;

namespace PurchaseService.API.Controllers
{
    [Route("purchase/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize]
        [HttpPost("addToCart")]
        public async Task<IActionResult> AddToCart([FromQuery]int listingId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
         

            int userId = int.Parse(userIdClaim.Value);
            var result = await _cartService.AddToCartAsync(listingId, userId);
            return Ok(result);
        }
        [Authorize]
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

      


    }
}

