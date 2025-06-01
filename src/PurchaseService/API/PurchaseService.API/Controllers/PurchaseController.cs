using Microsoft.AspNetCore.Mvc;
using PurchaseService.Domain.Repositories;

namespace PurchaseService.API.Controllers
{
    [Route("purchases/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository _repository;
       
        public PurchaseController(IPurchaseRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("getAllPurchases")]
        public async Task<IActionResult> GetAllPurchases()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("getPurchaseById")]
        public async Task<IActionResult> GetPurchaseById(int PurchaseId)
        {
            var result= await _repository.GetByIdAsync(PurchaseId);
            return Ok(result);
        }
    }
}
