using AVMLabs.Mvc.Models.Clients;
using AVMLabs.Mvc.Models.Tests;
using AVMLabs.Mvc.Models.WorkOrders;
using AVMLabs.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace AVMLabs.Mvc.Controllers
{
    public class WorkOrdersController : Controller
    {
        private readonly ApiService _apiService;

        public WorkOrdersController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<List<WorkOrderViewModel>>("api/workorders");

            return View(response?.Data ?? new List<WorkOrderViewModel>());
        }

        public async Task<IActionResult> Create()
        {
            var clientsResponse = await _apiService.GetAsync<List<ClientResponseModel>>("api/clients");
            var testsResponse = await _apiService.GetAsync<List<TestResponseModel>>("api/tests");

            ViewBag.Clients = clientsResponse?.Data ?? new List<ClientResponseModel>();
            ViewBag.Tests = testsResponse?.Data ?? new List<TestResponseModel>();

            return View(new CreateWorkOrderModel());
        }
    }
}