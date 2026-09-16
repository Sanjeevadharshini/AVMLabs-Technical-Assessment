namespace AVMLabs.Api.Models
{
    public class WorkOrderItem
    {
        public int WOItemId { get; set; }
        public int WOId { get; set; }
        public int TestId { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string SampleStatus { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }

        public WorkOrder WorkOrder { get; set; } = null!;
        public Test Test { get; set; } = null!;
    }
}