using AVMLabs.Mvc.Models.Clients;
using AVMLabs.Mvc.Models.Payments;
using AVMLabs.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace AVMLabs.Mvc.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApiService _apiService;

        public ClientsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string? search, string? country, int page = 1, int length = 10, bool ajax = false)
        {
            var response = await _apiService.GetAsync<List<ClientResponseModel>>("api/clients");
            var clients = response?.Data ?? new List<ClientResponseModel>();

            var countries = clients
                .Select(x => x.Country)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            if (!string.IsNullOrWhiteSpace(search))
                clients = clients.Where(x => x.ClientName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(country))
                clients = clients.Where(x => x.Country.Equals(country, StringComparison.OrdinalIgnoreCase)).ToList();

            var pageSize = length == 20 || length == 50 ? length : 10;

            var totalRecords = clients.Count;
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalRecords / (double)pageSize));

            if (page < 1)
                page = 1;

            if (page > totalPages)
                page = totalPages;

            clients = clients.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Search = search;
            ViewBag.Country = country;
            ViewBag.Countries = countries;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.PageSize = pageSize;

            if (ajax)
                return PartialView("_ClientList", clients);

            return View(clients);
        }

        public async Task<IActionResult> Ledger(int id)
        {
            var response = await _apiService.GetAsync<LedgerResponseModel>($"api/clients/{id}/ledger");

            if (response == null || !response.Success || response.Data == null)
                return NotFound();

            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> RecordPayment([FromBody] CreatePaymentModel model)
        {
            var response = await _apiService.PostAsync<object>("api/payments", model);

            if (response == null)
                return BadRequest(new { success = false, message = "Unable to record payment." });

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}