namespace AVMLabs.Api.DTOs.Reports
{
    public class OutstandingClientDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public decimal CreditLimit { get; set; }
        public decimal CurrentOutstanding { get; set; }
        public decimal ExcessAmount { get; set; }
    }
}