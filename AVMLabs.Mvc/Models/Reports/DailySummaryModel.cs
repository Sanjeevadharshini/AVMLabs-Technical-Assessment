namespace AVMLabs.Mvc.Models.Reports
{
    public class DailySummaryModel
    {
        public DateTime Date { get; set; }
        public int WorkOrderCount { get; set; }
        public int TestCount { get; set; }
        public decimal Revenue { get; set; }
    }
}