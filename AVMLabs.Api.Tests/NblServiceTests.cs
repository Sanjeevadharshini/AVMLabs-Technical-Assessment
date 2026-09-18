using AVMLabs.Api.Data;
using AVMLabs.Api.Models;
using AVMLabs.Api.Services;
using AVMLabs.Api.Services.Interfaces;
using AVMLabs.Api.Services.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AVMLabs.Api.Tests
{
    public class NblServiceTests
    {
        private static AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetNblStatusAsync_NoOutstanding_ReturnsNbl()
        {
            await using var context = CreateContext();

            context.Clients.Add(new Client
            {
                ClientId = 1,
                ClientName = "Test Client",
                CreditLimit = 10000,
                IsActive = true
            });

            await context.SaveChangesAsync();

            var calculator = new Mock<IOutstandingCalculator>();

            calculator
                .Setup(x => x.GetForClientAsync(1))
                .ReturnsAsync(new OutstandingAmounts
                {
                    PendingInvoiceAmount = 0,
                    InTransitAmount = 0
                });

            var service = new NblService(context, calculator.Object);

            var result = await service.GetNblStatusAsync(1);

            Assert.True(result.IsNbl);
        }

        [Fact]
        public async Task GetNblStatusAsync_PendingInvoiceOutstanding_ReturnsNonNbl()
        {
            await using var context = CreateContext();

            context.Clients.Add(new Client
            {
                ClientId = 1,
                ClientName = "Test Client",
                CreditLimit = 10000,
                IsActive = true
            });

            await context.SaveChangesAsync();

            var calculator = new Mock<IOutstandingCalculator>();

            calculator
                .Setup(x => x.GetForClientAsync(1))
                .ReturnsAsync(new OutstandingAmounts
                {
                    PendingInvoiceAmount = 1000,
                    InTransitAmount = 0
                });

            var service = new NblService(context, calculator.Object);

            var result = await service.GetNblStatusAsync(1);

            Assert.False(result.IsNbl);
        }

        [Fact]
        public async Task GetNblStatusAsync_InTransitOutstanding_ReturnsNonNbl()
        {
            await using var context = CreateContext();

            context.Clients.Add(new Client
            {
                ClientId = 1,
                ClientName = "Test Client",
                CreditLimit = 10000,
                IsActive = true
            });

            await context.SaveChangesAsync();

            var calculator = new Mock<IOutstandingCalculator>();

            calculator
                .Setup(x => x.GetForClientAsync(1))
                .ReturnsAsync(new OutstandingAmounts
                {
                    PendingInvoiceAmount = 0,
                    InTransitAmount = 1500
                });

            var service = new NblService(context, calculator.Object);

            var result = await service.GetNblStatusAsync(1);

            Assert.False(result.IsNbl);
        }
    }
}