namespace AVMLabs.Api.DTOs.Payments
{
    public class PaymentResponseDto
    {
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Mode { get; set; } = string.Empty;
        public decimal GatewayFee { get; set; }
        public decimal NetAmount { get; set; }
        public List<PaymentResponseItemDto> Allocations { get; set; } = new();
    }
}