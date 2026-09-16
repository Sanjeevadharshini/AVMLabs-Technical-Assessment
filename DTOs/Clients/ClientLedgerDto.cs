namespace AVMLabs.Api.DTOs.Clients
{
    public class ClientLedgerDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public decimal CreditLimit { get; set; }
        public decimal TotalOutstanding { get; set; }
        public bool IsNbl { get; set; }
        public decimal TotalInvoiced { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalGatewayFees { get; set; }
        public decimal NetOutstanding { get; set; }
        public List<LedgerEntryDto> Entries { get; set; } = new();
    }
}