using AVMLabs.Api.DTOs.Common;
using AVMLabs.Api.DTOs.Payments;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AVMLabs.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PaymentResponseDto>>> CreatePayment(CreatePaymentDto dto)
        {
            var payment = await _paymentService.CreatePaymentAsync(dto);

            return StatusCode(201, ApiResponse<PaymentResponseDto>.SuccessResponse(payment, "Payment created successfully."));
        }
    }
}