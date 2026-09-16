using AVMLabs.Api.DTOs.Payments;

namespace AVMLabs.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreatePaymentAsync(CreatePaymentDto dto);
    }
}