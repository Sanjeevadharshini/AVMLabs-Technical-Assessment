using AVMLabs.Api.Common;
using AVMLabs.Api.Models;
using AVMLabs.Api.Data;
using AVMLabs.Api.DTOs.Payments;
using AVMLabs.Api.Exceptions;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AVMLabs.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;
        private readonly IOutstandingCalculator _outstandingCalculator;

        public PaymentService(AppDbContext context, IOutstandingCalculator outstandingCalculator)
        {
            _context = context;
            _outstandingCalculator = outstandingCalculator;
        }

        public async Task<PaymentResponseDto> CreatePaymentAsync(CreatePaymentDto dto)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(x => x.ClientId == dto.ClientId);

            if (client == null)
                throw new NotFoundException("Client not found.");

            if (dto.Amount <= 0)
                throw new BusinessException("Payment amount must be greater than zero.");

            var outstanding = await _outstandingCalculator.GetForClientAsync(dto.ClientId);

            if (outstanding.PendingInvoiceAmount <= 0)
                throw new BusinessException("Client has no pending invoices to apply this payment to.");

            if (dto.Amount > outstanding.PendingInvoiceAmount)
                throw new BusinessException($"Payment amount cannot exceed the total pending invoice amount of {outstanding.PendingInvoiceAmount:0.00}.");

            var invoices = await _context.Invoices
                .Where(x => x.ClientId == dto.ClientId && x.Status != Constants.InvoiceStatus.Paid)
                .OrderBy(x => x.InvoiceDate)
                .ThenBy(x => x.InvoiceId)
                .ToListAsync();

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var payments = new List<Payment>();
            var paymentAmount = dto.Amount;

            try
            {
                foreach (var invoice in invoices)
                {
                    if (paymentAmount <= 0)
                        break;

                    var paidAmount = await _context.Payments
                        .Where(x => x.InvoiceId == invoice.InvoiceId)
                        .SumAsync(x => (decimal?)x.Amount) ?? 0;

                    var outstandingAmount = invoice.TotalAmount - paidAmount;

                    if (outstandingAmount <= 0)
                    {
                        invoice.Status = Constants.InvoiceStatus.Paid;
                        continue;
                    }

                    var amountToApply = Math.Min(paymentAmount, outstandingAmount);

                    var gatewayFee = dto.Mode.Equals(Constants.PaymentMode.Online, StringComparison.OrdinalIgnoreCase)
                        ? Math.Round(amountToApply * 0.02m, 2)
                        : 0;

                    var netAmount = amountToApply - gatewayFee;

                    var payment = new Payment
                    {
                        InvoiceId = invoice.InvoiceId,
                        PaymentDate = dto.PaymentDate,
                        Amount = amountToApply,
                        Mode = dto.Mode,
                        GatewayFee = gatewayFee,
                        NetAmount = netAmount,
                        CreatedOn = DateTime.UtcNow
                    };

                    _context.Payments.Add(payment);
                    payments.Add(payment);

                    paymentAmount -= amountToApply;

                    if (amountToApply >= outstandingAmount)
                        invoice.Status = Constants.InvoiceStatus.Paid;
                }

                if (paymentAmount > 0)
                    throw new BusinessException("Payment amount exceeds the total outstanding amount.");

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new PaymentResponseDto
                {
                    PaymentDate = dto.PaymentDate,
                    Amount = dto.Amount,
                    Mode = dto.Mode,
                    GatewayFee = payments.Sum(x => x.GatewayFee),
                    NetAmount = payments.Sum(x => x.NetAmount),
                    Allocations = payments.Select(x => new PaymentResponseItemDto
                    {
                        PaymentId = x.PaymentId,
                        InvoiceId = x.InvoiceId,
                        Amount = x.Amount,
                        GatewayFee = x.GatewayFee,
                        NetAmount = x.NetAmount
                    }).ToList()
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}