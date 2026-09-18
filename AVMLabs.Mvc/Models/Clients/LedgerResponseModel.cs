namespace AVMLabs.Mvc.Models.Clients
{
    public class LedgerResponseModel
    {
        public string ClientName { get; set; } = string.Empty;
        public decimal CreditLimit { get; set; }
        public decimal TotalOutstanding { get; set; }
        public bool IsNbl { get; set; }
        public decimal TotalInvoiced { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalGatewayFees { get; set; }
        public decimal NetOutstanding { get; set; }
        public List<LedgerEntryModel> Entries { get; set; } = new();
    }

    public class LedgerEntryModel
    {
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal RunningBalance { get; set; }
        public string EntryType { get; set; } = string.Empty;
    }
}