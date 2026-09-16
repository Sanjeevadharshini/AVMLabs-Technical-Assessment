namespace AVMLabs.Api.Services.Models
{
    public class OutstandingAmounts
    {
        public decimal PendingInvoiceAmount { get; set; }
        public decimal InTransitAmount { get; set; }
        public decimal Total => PendingInvoiceAmount + InTransitAmount;
    }
}