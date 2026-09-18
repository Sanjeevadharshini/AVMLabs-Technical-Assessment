using AVMLabs.Mvc.Models.Common;
using AVMLabs.Mvc.Models.Reports;
using AVMLabs.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace AVMLabs.Mvc.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApiService _apiService;

        public ReportsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Dashboard(DateTime? fromDate, DateTime? toDate)
        {
            var from = fromDate?.Date ?? DateTime.Today.AddDays(-30);
            var to = toDate?.Date ?? DateTime.Today;

            var dailyResponse = await _apiService.GetAsync<List<DailySummaryModel>>($"api/reports/daily-summary?fromDate={from:yyyy-MM-dd}&toDate={to:yyyy-MM-dd}");

            var outstandingResponse = await _apiService.GetAsync<List<OutstandingClientModel>>("api/reports/outstanding-clients");

            var dailySummary = dailyResponse?.Data ?? new List<DailySummaryModel>();
            var outstandingClients = outstandingResponse?.Data ?? new List<OutstandingClientModel>();

            var model = new DashboardModel
            {
                FromDate = from,
                ToDate = to,
                TotalWorkOrders = dailySummary.Sum(x => x.WorkOrderCount),
                TotalRevenue = dailySummary.Sum(x => x.Revenue),
                ClientsOverCreditLimit = outstandingClients.Count,
                DailySummary = dailySummary,
                OutstandingClients = outstandingClients.Take(5).ToList()
            };

            return View(model);
        }
    }
}