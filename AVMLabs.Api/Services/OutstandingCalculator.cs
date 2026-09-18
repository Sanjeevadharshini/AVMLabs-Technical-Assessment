using AVMLabs.Api.Common;
using AVMLabs.Api.Data;
using AVMLabs.Api.Services.Interfaces;
using AVMLabs.Api.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace AVMLabs.Api.Services
{
    public class OutstandingCalculator : IOutstandingCalculator
    {
        private readonly AppDbContext _context;

        public OutstandingCalculator(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OutstandingAmounts> GetForClientAsync(int clientId)
        {
            var pending = await _context.Invoices
                .AsNoTracking()
                .Where(i => i.ClientId == clientId && i.Status == Constants.InvoiceStatus.Pending)
                .Select(i => i.TotalAmount - i.Payments.Sum(p => p.Amount))
                .Where(x => x > 0)
                .SumAsync();

            var inTransit = await _context.WorkOrderItems
                .AsNoTracking()
                .Where(wi => wi.WorkOrder.ClientId == clientId && wi.SampleStatus == Constants.SampleStatus.InTransit 
                             && !_context.Invoices.Any(i => i.WOId == wi.WOId))
                .SumAsync(wi => (decimal?)wi.Amount) ?? 0;

            return new OutstandingAmounts { PendingInvoiceAmount = pending, InTransitAmount = inTransit };
        }

        public async Task<Dictionary<int, OutstandingAmounts>> GetForAllClientsAsync()
        {
            var pendingByClient = await _context.Invoices
                .AsNoTracking()
                .Where(i => i.Status == Constants.InvoiceStatus.Pending)
                .Select(i => new { i.ClientId, Outstanding = i.TotalAmount - i.Payments.Sum(p => p.Amount) })
                .Where(x => x.Outstanding > 0)
                .GroupBy(x => x.ClientId)
                .Select(g => new { ClientId = g.Key, Amount = g.Sum(x => x.Outstanding) })
                .ToDictionaryAsync(x => x.ClientId, x => x.Amount);

            var inTransitByClient = await _context.WorkOrderItems
                .AsNoTracking()
                .Where(wi => wi.SampleStatus == Constants.SampleStatus.InTransit && !_context.Invoices.Any(i => i.WOId == wi.WOId))
                .GroupBy(wi => wi.WorkOrder.ClientId)
                .Select(g => new { ClientId = g.Key, Amount = g.Sum(wi => wi.Amount) })
                .ToDictionaryAsync(x => x.ClientId, x => x.Amount);

            var clientIds = pendingByClient.Keys.Union(inTransitByClient.Keys);

            var result = new Dictionary<int, OutstandingAmounts>();

            foreach (var id in clientIds)
            {
                result[id] = new OutstandingAmounts
                {
                    PendingInvoiceAmount = pendingByClient.TryGetValue(id, out var p) ? p : 0,
                    InTransitAmount = inTransitByClient.TryGetValue(id, out var it) ? it : 0
                };
            }

            return result;
        }
    }
}