namespace AVMLabs.Api.DTOs.WorkOrders
{
    public class InTransitWorkOrderDto
    {
        public int WOId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public DateTime WODate { get; set; }
        public int HoursElapsed { get; set; }
    }
}