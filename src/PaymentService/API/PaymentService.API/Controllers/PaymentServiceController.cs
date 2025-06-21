using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Services;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
       
        [HttpPost]
        public async Task<ActionResult<PaymentResult>> MakePayment([FromBody] PaymentRequest request)
        {
            var result = await _paymentService.ProcessPayment(request);
            return Ok(result);
        }
    }
}

