namespace AVMLabs.Api.DTOs.Reports
{
    public class DailySummaryDto
    {
        public DateTime Date { get; set; }
        public int WorkOrderCount { get; set; }
        public int TestCount { get; set; }
        public decimal Revenue { get; set; }
    }
}