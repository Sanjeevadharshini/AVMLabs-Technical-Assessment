using AVMLabs.Api.DTOs.Reports;

namespace AVMLabs.Api.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<DailySummaryDto>> GetDailySummaryAsync(DateTime fromDate, DateTime toDate);
        Task<List<OutstandingClientDto>> GetOutstandingClientsAsync();
        Task<List<GatewayFeeDto>> GetGatewayFeesAsync(DateTime fromDate, DateTime toDate);
    }
}