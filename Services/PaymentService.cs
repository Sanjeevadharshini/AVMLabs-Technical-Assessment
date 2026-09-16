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

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentResponseDto> CreatePaymentAsync(CreatePaymentDto dto)
        {
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(x => x.InvoiceId == dto.InvoiceId);

            if (invoice == null)
                throw new NotFoundException("Invoice not found.");

            if (dto.Amount <= 0)
                throw new BusinessException("Payment amount must be greater than zero.");

            if (invoice.Status == Constants.InvoiceStatus.Paid)
                throw new BusinessException("Invoice is already paid.");

            var paidAmount = await _context.Payments
                .Where(x => x.InvoiceId == dto.InvoiceId)
                .SumAsync(x => (decimal?)x.Amount) ?? 0;

            var outstandingAmount = invoice.TotalAmount - paidAmount;

            if (dto.Amount > outstandingAmount)
                throw new BusinessException($"Payment amount cannot exceed the outstanding invoice amount of {outstandingAmount:0.00}.");

            var gatewayFee = dto.Mode.Equals(Constants.PaymentMode.Online, StringComparison.OrdinalIgnoreCase)
                ? Math.Round(dto.Amount * 0.02m, 2)
                : 0;

            var netAmount = dto.Amount - gatewayFee;

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var payment = new Payment
            {
                InvoiceId = dto.InvoiceId,
                PaymentDate = dto.PaymentDate,
                Amount = dto.Amount,
                Mode = dto.Mode,
                GatewayFee = gatewayFee,
                NetAmount = netAmount,
                CreatedOn = DateTime.UtcNow
            };

            _context.Payments.Add(payment);

            var totalPaid = paidAmount + dto.Amount;

            if (totalPaid >= invoice.TotalAmount)
                invoice.Status = Constants.InvoiceStatus.Paid;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new PaymentResponseDto
            {
                PaymentId = payment.PaymentId,
                InvoiceId = payment.InvoiceId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                Mode = payment.Mode,
                GatewayFee = payment.GatewayFee,
                NetAmount = payment.NetAmount
            };
        }
    }
}