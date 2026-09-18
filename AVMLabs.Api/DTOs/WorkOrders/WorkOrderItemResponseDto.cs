namespace AVMLabs.Api.DTOs.WorkOrders
{
    public class WorkOrderItemResponseDto
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