namespace AVMLabs.Mvc.Models.Reports
{
    public class DashboardModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalWorkOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ClientsOverCreditLimit { get; set; }
        public List<DailySummaryModel> DailySummary { get; set; } = new();
        public List<OutstandingClientModel> OutstandingClients { get; set; } = new();
    }
}