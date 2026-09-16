using AVMLabs.Api.Data;
using AVMLabs.Api.Models;
using AVMLabs.Api.DTOs.Clients;
using AVMLabs.Api.Exceptions;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AVMLabs.Api.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;
        private readonly INblService _nblService;
        private readonly IOutstandingCalculator _calculator;

        public ClientService(AppDbContext context, INblService nblService, IOutstandingCalculator calculator)
        {
            _context = context;
            _nblService = nblService;
            _calculator = calculator;
        }
        public async Task<List<ClientResponseDto>> GetClientsAsync()
        {
            var clients = await _context.Clients
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();

            var amountsByClient = await _calculator.GetForAllClientsAsync();

            return clients.Select(client =>
            {
                var amounts = amountsByClient.TryGetValue(client.ClientId, out var a) ? a : new Models.OutstandingAmounts();

                return new ClientResponseDto
                {
                    ClientId = client.ClientId,
                    ClientName = client.ClientName,
                    ContactPerson = client.ContactPerson,
                    Phone = client.Phone,
                    Email = client.Email,
                    City = client.City,
                    Country = client.Country,
                    CreditLimit = client.CreditLimit,
                    IsActive = client.IsActive,
                    OutstandingBalance = amounts.Total,
                    IsNbl = amounts.Total <= 0
                };
            }).ToList();
        }
        public async Task<ClientLedgerDto> GetClientLedgerAsync(int clientId)
        {
            var client = await _context.Clients
                 .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClientId == clientId);

            if (client == null)
                throw new NotFoundException("Client not found.");

            var invoices = await _context.Invoices
                 .AsNoTracking()
                .Where(x => x.ClientId == clientId)
                .ToListAsync();

            var payments = await _context.Payments
                 .AsNoTracking()
                .Include(x => x.Invoice)
                .Where(x => x.Invoice.ClientId == clientId)
                .ToListAsync();

            var entries = new List<LedgerEntryDto>();

            foreach (var invoice in invoices)
            {
                entries.Add(new LedgerEntryDto
                {
                    Date = invoice.InvoiceDate,
                    Description = $"Invoice #{invoice.InvoiceId}",
                    Debit = invoice.TotalAmount,
                    Credit = 0
                });
            }

            foreach (var payment in payments)
            {
                entries.Add(new LedgerEntryDto
                {
                    Date = payment.PaymentDate,
                    Description = $"Payment #{payment.PaymentId}",
                    Debit = 0,
                    Credit = payment.Amount
                });

                if (payment.GatewayFee > 0)
                {
                    entries.Add(new LedgerEntryDto
                    {
                        Date = payment.PaymentDate,
                        Description = $"Gateway Fee - Payment #{payment.PaymentId}",
                        Debit = payment.GatewayFee,
                        Credit = 0
                    });
                }
            }

            entries = entries.OrderBy(x => x.Date).ToList();

            decimal runningBalance = 0;

            foreach (var entry in entries)
            {
                runningBalance += entry.Debit - entry.Credit;
                entry.RunningBalance = runningBalance;
            }

            var nbl = await _nblService.GetNblStatusAsync(clientId);

            return new ClientLedgerDto
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                CreditLimit = client.CreditLimit,
                TotalOutstanding = nbl.TotalOutstanding,
                IsNbl = nbl.IsNbl,
                TotalInvoiced = invoices.Sum(x => x.TotalAmount),
                TotalPaid = payments.Sum(x => x.Amount),
                TotalGatewayFees = payments.Sum(x => x.GatewayFee),
                NetOutstanding = runningBalance,
                Entries = entries
            };
        }

        public async Task<ClientResponseDto> CreateClientAsync(CreateClientDto dto)
        {
            if (dto.CreditLimit <= 0)
                throw new BusinessException("Credit limit must be greater than zero.");

            var emailExists = await _context.Clients
                .AnyAsync(x => x.Email == dto.Email);

            if (emailExists)
                throw new BusinessException("Email already exists.");

            var client = new Client
            {
                ClientName = dto.ClientName,
                ContactPerson = dto.ContactPerson,
                Phone = dto.Phone,
                Email = dto.Email,
                City = dto.City,
                Country = dto.Country,
                CreditLimit = dto.CreditLimit,
                IsActive = true,
                CreatedOn = DateTime.UtcNow
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return new ClientResponseDto
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                ContactPerson = client.ContactPerson,
                Phone = client.Phone,
                Email = client.Email,
                City = client.City,
                Country = client.Country,
                CreditLimit = client.CreditLimit,
                IsActive = client.IsActive,
                OutstandingBalance = 0,
                IsNbl = true
            };
        }

        public async Task<ClientResponseDto> ToggleClientAsync(int clientId)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(x => x.ClientId == clientId);

            if (client == null)
                throw new NotFoundException("Client not found.");

            client.IsActive = !client.IsActive;
            client.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var nbl = await _nblService.GetNblStatusAsync(clientId);

            return new ClientResponseDto
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                ContactPerson = client.ContactPerson,
                Phone = client.Phone,
                Email = client.Email,
                City = client.City,
                Country = client.Country,
                CreditLimit = client.CreditLimit,
                IsActive = client.IsActive,
                OutstandingBalance = nbl.TotalOutstanding,
                IsNbl = nbl.IsNbl
            };
        }
    }
}