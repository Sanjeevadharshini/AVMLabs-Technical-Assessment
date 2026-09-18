namespace AVMLabs.Mvc.Models.WorkOrders
{
    public class WorkOrderViewModel
    {
        public int WOId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime WODate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public List<WorkOrderItemViewModel> Items { get; set; } = new();
    }

    public class WorkOrderItemViewModel
    {
        public int WOItemId { get; set; }
        public int TestId { get; set; }
        public string TestCode { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string SampleStatus { get; set; } = string.Empty;
    }
}