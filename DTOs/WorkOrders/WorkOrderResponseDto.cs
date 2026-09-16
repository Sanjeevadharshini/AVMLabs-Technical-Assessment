namespace AVMLabs.Api.DTOs.WorkOrders
{
    public class WorkOrderResponseDto
    {
        public int WOId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime WODate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public List<WorkOrderItemResponseDto> Items { get; set; } = new();
    }
}