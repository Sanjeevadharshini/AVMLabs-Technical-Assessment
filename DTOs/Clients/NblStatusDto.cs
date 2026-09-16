namespace AVMLabs.Api.DTOs.Clients
{
    public class NblStatusDto
    {
        public bool IsNbl { get; set; }
        public decimal OutstandingInvoiceAmount { get; set; }
        public decimal InTransitWorkOrderAmount { get; set; }
        public decimal TotalOutstanding { get; set; }
    }
}