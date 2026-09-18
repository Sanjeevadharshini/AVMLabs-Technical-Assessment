namespace AVMLabs.Api.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int ClientId { get; set; }
        public int WOId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }

        public Client Client { get; set; } = null!;
        public WorkOrder WorkOrder { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}