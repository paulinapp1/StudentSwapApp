using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseService.Application;
using PurchaseService.Application.DTO;
using PurchaseService.Domain.Repositories;
using System.Security.Claims;

namespace PurchaseService.API.Controllers
{
    [Route("purchases/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository _repository;
        private readonly IPurchaseService _purchaseService;
       
        public PurchaseController(IPurchaseRepository repository, IPurchaseService purchaseService)
        {
            _repository = repository;
            _purchaseService = purchaseService;
        }
  
        [HttpDelete("cancelPurchase")]
        public async Task<IActionResult> CancelPurchase([FromQuery] int purchaseId)
        {
            var result =  await _purchaseService.CancelPurchase(purchaseId);
            return Ok(result);
        }
        [HttpGet("getAllPurchases")]
        public async Task<IActionResult> GetAllPurchases()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpGet("getPurchaseById")]
        public async Task<IActionResult> GetPurchaseById(int PurchaseId)
        {
            var result= await _repository.GetByIdAsync(PurchaseId);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("createPurchase")]
        public async Task<IActionResult> CreatePurchase([FromBody] CreatePurchaseRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);
            var paymentRequest = new CreatePurchaseRequest
            {
                CardNumber = request.CardNumber,
                CardExpiry = request.CardExpiry,
                CardCvv = request.CardCvv
            };
            var result = await _purchaseService.CreatePurchase(request.ListingId, userId, paymentRequest);
            return Ok(result);

        }

    }
}
