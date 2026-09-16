using AVMLabs.Api.Data;
using AVMLabs.Api.DTOs.Clients;
using AVMLabs.Api.Exceptions;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AVMLabs.Api.Services
{
    public class NblService : INblService
    {
        private readonly AppDbContext _context;
        private readonly IOutstandingCalculator _calculator;

        public NblService(AppDbContext context, IOutstandingCalculator calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        public async Task<NblStatusDto> GetNblStatusAsync(int clientId)
        {
            var clientExists = await _context.Clients.AnyAsync(x => x.ClientId == clientId);

            if (!clientExists)
                throw new NotFoundException("Client not found.");

            var amounts = await _calculator.GetForClientAsync(clientId);

            return new NblStatusDto
            {
                IsNbl = amounts.Total <= 0,
                OutstandingInvoiceAmount = amounts.PendingInvoiceAmount,
                InTransitWorkOrderAmount = amounts.InTransitAmount,
                TotalOutstanding = amounts.Total
            };
        }
    }
}