namespace AVMLabs.Api.Models
{
    public class WorkOrder
    {
        public int WOId { get; set; }
        public int ClientId { get; set; }
        public DateTime WODate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }

        public Client Client { get; set; } = null!;
        public ICollection<WorkOrderItem> WorkOrderItems { get; set; } = new List<WorkOrderItem>();
    }
}