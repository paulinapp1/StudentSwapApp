using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTO;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Services;
using PaymentService.Domain.Interfaces;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ITransactionRepository _repository;

        public PaymentsController(IPaymentService paymentService, ITransactionRepository repository)
        {
            _paymentService = paymentService;
            _repository = repository;
        }
       
        [HttpPost]
        public async Task<ActionResult<PaymentResult>> MakePayment([FromBody] PaymentRequest request)
        {
            var result = await _paymentService.ProcessPayment(request);
            return Ok(result);
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var result = await _repository.GetAllTransactions();
            return Ok(result);

        }
    }
}

