namespace AVMLabs.Api.DTOs.Payments
{
    public class PaymentResponseItemDto
    {
        public int PaymentId { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public decimal GatewayFee { get; set; }
        public decimal NetAmount { get; set; }
    }
}