using AVMLabs.Api.DTOs.Common;
using AVMLabs.Api.DTOs.Reports;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AVMLabs.Api.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("daily-summary")]
        public async Task<ActionResult<ApiResponse<List<DailySummaryDto>>>> GetDailySummary(DateTime fromDate, DateTime toDate)
        {
            var summary = await _reportService.GetDailySummaryAsync(fromDate, toDate);

            return Ok(ApiResponse<List<DailySummaryDto>>.SuccessResponse(summary, "Daily summary retrieved successfully."));
        }

        [HttpGet("outstanding-clients")]
        public async Task<ActionResult<ApiResponse<List<OutstandingClientDto>>>> GetOutstandingClients()
        {
            var clients = await _reportService.GetOutstandingClientsAsync();

            return Ok(ApiResponse<List<OutstandingClientDto>>.SuccessResponse(clients, "Outstanding clients retrieved successfully."));
        }

        [HttpGet("gateway-fees")]
        public async Task<ActionResult<ApiResponse<List<GatewayFeeDto>>>> GetGatewayFees(DateTime fromDate, DateTime toDate)
        {
            var fees = await _reportService.GetGatewayFeesAsync(fromDate, toDate);

            return Ok(ApiResponse<List<GatewayFeeDto>>.SuccessResponse(fees, "Gateway fees retrieved successfully."));
        }
    }
}