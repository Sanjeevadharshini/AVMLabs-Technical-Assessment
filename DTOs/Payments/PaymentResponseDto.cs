namespace AVMLabs.Api.DTOs.Payments
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public int InvoiceId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Mode { get; set; } = string.Empty;
        public decimal GatewayFee { get; set; }
        public decimal NetAmount { get; set; }
    }
}