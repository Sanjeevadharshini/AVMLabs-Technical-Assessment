namespace AVMLabs.Mvc.Models.Payments
{
    public class CreatePaymentModel
    {
        public int ClientId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Mode { get; set; } = string.Empty;
    }
}