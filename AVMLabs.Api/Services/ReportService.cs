using AVMLabs.Api.Common;
using AVMLabs.Api.Data;
using AVMLabs.Api.DTOs.Reports;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AVMLabs.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;
        private readonly IOutstandingCalculator _calculator;

        public ReportService(AppDbContext context, IOutstandingCalculator calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        public async Task<List<OutstandingClientDto>> GetOutstandingClientsAsync()
        {
            var amountsByClient = await _calculator.GetForAllClientsAsync();
            var clients = await _context.Clients.AsNoTracking().ToListAsync();

            return clients
                .Select(c =>
                {
                    var amounts = amountsByClient.TryGetValue(c.ClientId, out var a) ? a : new Models.OutstandingAmounts();
                    var outstanding = amounts.Total;

                    return new OutstandingClientDto
                    {
                        ClientId = c.ClientId,
                        ClientName = c.ClientName,
                        CreditLimit = c.CreditLimit,
                        CurrentOutstanding = outstanding,
                        ExcessAmount = outstanding - c.CreditLimit
                    };
                })
                .Where(x => x.CurrentOutstanding > x.CreditLimit)
                .OrderByDescending(x => x.ExcessAmount)
                .ToList();
        }
        public async Task<List<DailySummaryDto>> GetDailySummaryAsync(DateTime fromDate, DateTime toDate)
        {
            if (fromDate.Date > toDate.Date)
                throw new Exceptions.BusinessException("From date cannot be greater than to date.");

            var start = fromDate.Date;
            var end = toDate.Date;
            var endExclusive = end.AddDays(1);

            var workOrders = await _context.WorkOrders
                .AsNoTracking()
                .Where(x => x.WODate >= start && x.WODate < endExclusive)
                .Include(x => x.WorkOrderItems)
                .ToListAsync();

            var summaryByDate = workOrders
                .GroupBy(x => x.WODate.Date)
                .ToDictionary(
                    g => g.Key,
                    g => new DailySummaryDto
                    {
                        Date = g.Key,
                        WorkOrderCount = g.Count(),
                        TestCount = g.SelectMany(y => y.WorkOrderItems).Sum(y => y.Quantity),
                        Revenue = g.Sum(y => y.TotalAmount)
                    });

            var result = new List<DailySummaryDto>();

            for (var date = start; date <= end; date = date.AddDays(1))
            {
                result.Add(summaryByDate.TryGetValue(date, out var existing)
                    ? existing
                    : new DailySummaryDto { Date = date, WorkOrderCount = 0, TestCount = 0, Revenue = 0 });
            }

            return result;
        }
        public async Task<List<GatewayFeeDto>> GetGatewayFeesAsync(DateTime fromDate, DateTime toDate)
        {
            if (fromDate.Date > toDate.Date)
                throw new Exceptions.BusinessException("From date cannot be greater than to date.");

            var endDate = toDate.Date.AddDays(1);

            return await _context.Payments
                .AsNoTracking()
                .Where(x => x.PaymentDate >= fromDate.Date && x.PaymentDate < endDate)
                .GroupBy(x => x.Mode)
                .Select(x => new GatewayFeeDto
                {
                    Mode = x.Key,
                    TotalGatewayFees = x.Sum(y => y.GatewayFee)
                })
                .OrderBy(x => x.Mode)
                .ToListAsync();
        }
    }
}