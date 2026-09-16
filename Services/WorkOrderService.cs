using AVMLabs.Api.Common;
using AVMLabs.Api.Models;
using AVMLabs.Api.Data;
using AVMLabs.Api.DTOs.WorkOrders;
using AVMLabs.Api.Exceptions;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AVMLabs.Api.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly AppDbContext _context;
        private readonly INblService _nblService;

        public WorkOrderService(AppDbContext context, INblService nblService)
        {
            _context = context;
            _nblService = nblService;
        }

        public async Task<WorkOrderResponseDto> CreateWorkOrderAsync(CreateWorkOrderDto dto)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(x => x.ClientId == dto.ClientId);

            if (client == null)
                throw new NotFoundException("Client not found.");

            if (!client.IsActive)
                throw new BusinessException("Cannot create work order for an inactive client.");

            var testIds = dto.Items.Select(x => x.TestId).Distinct().ToList();

            var tests = await _context.Tests
                .Where(x => testIds.Contains(x.TestId) && x.IsActive)
                .ToListAsync();

            if (tests.Count != testIds.Count)
                throw new BusinessException("One or more tests are invalid or inactive.");

            var testLookup = tests.ToDictionary(x => x.TestId);

            var nbl = await _nblService.GetNblStatusAsync(dto.ClientId);

            if (!nbl.IsNbl)
                throw new BusinessException("Work order cannot be created because the client has outstanding dues.");

            var workOrder = new WorkOrder
            {
                ClientId = dto.ClientId,
                WODate = dto.WODate,
                Status = Constants.WorkOrderStatus.Pending,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            foreach (var item in dto.Items)
            {
                var test = testLookup[item.TestId];
                var amount = item.Quantity * test.Rate;

                workOrder.WorkOrderItems.Add(new WorkOrderItem
                {
                    TestId = test.TestId,
                    Quantity = item.Quantity,
                    Rate = test.Rate,
                    Amount = amount,
                    SampleStatus = Constants.SampleStatus.InTransit,
                    CreatedOn = DateTime.UtcNow
                });

                workOrder.TotalAmount += amount;
            }

            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync();

            return MapToResponse(workOrder, client);
        }

        public async Task<List<InTransitWorkOrderDto>> GetInTransitWorkOrdersAsync()
        {
            var workOrderItems = await _context.WorkOrderItems
                .Include(x => x.WorkOrder)
                .ThenInclude(x => x.Client)
                .Include(x => x.Test)
                .Where(x => x.SampleStatus == Constants.SampleStatus.InTransit)
                .OrderBy(x => x.WorkOrder.WODate)
                .ToListAsync();

            return workOrderItems.Select(x => new InTransitWorkOrderDto
            {
                WOId = x.WOId,
                ClientName = x.WorkOrder.Client.ClientName,
                TestName = x.Test.TestName,
                WODate = x.WorkOrder.WODate,
                HoursElapsed = (int)(DateTime.UtcNow - x.WorkOrder.WODate).TotalHours
            }).ToList();
        }

        public async Task<WorkOrderResponseDto> UpdateStatusAsync(int woId, UpdateWorkOrderStatusDto dto)
        {
            var workOrder = await _context.WorkOrders
                .Include(x => x.Client)
                .Include(x => x.WorkOrderItems)
                .ThenInclude(x => x.Test)
                .FirstOrDefaultAsync(x => x.WOId == woId);

            if (workOrder == null)
                throw new NotFoundException("Work order not found.");

            var allowedStatuses = new[]
            {
                Constants.WorkOrderStatus.Pending,
                Constants.WorkOrderStatus.Processing,
                Constants.WorkOrderStatus.Reported,
                Constants.WorkOrderStatus.Billed
            };

            if (!allowedStatuses.Contains(dto.Status))
                throw new BusinessException("Invalid work order status.");

            var validTransition = workOrder.Status switch
            {
                Constants.WorkOrderStatus.Pending => dto.Status == Constants.WorkOrderStatus.Processing,
                Constants.WorkOrderStatus.Processing => dto.Status == Constants.WorkOrderStatus.Reported,
                Constants.WorkOrderStatus.Reported => dto.Status == Constants.WorkOrderStatus.Billed,
                _ => false
            };

            if (!validTransition)
                throw new BusinessException($"Invalid status transition from {workOrder.Status} to {dto.Status}.");

            workOrder.Status = dto.Status;

            await _context.SaveChangesAsync();

            return MapToResponse(workOrder, workOrder.Client);
        }

        private static WorkOrderResponseDto MapToResponse(WorkOrder workOrder, Client client)
        {
            return new WorkOrderResponseDto
            {
                WOId = workOrder.WOId,
                ClientId = workOrder.ClientId,
                ClientName = client.ClientName,
                WODate = workOrder.WODate,
                Status = workOrder.Status,
                TotalAmount = workOrder.TotalAmount,
                CreatedBy = workOrder.CreatedBy,
                Items = workOrder.WorkOrderItems.Select(x => new WorkOrderItemResponseDto
                {
                    WOItemId = x.WOItemId,
                    TestId = x.TestId,
                    TestCode = x.Test.TestCode,
                    TestName = x.Test.TestName,
                    Quantity = x.Quantity,
                    Rate = x.Rate,
                    Amount = x.Amount,
                    SampleStatus = x.SampleStatus
                }).ToList()
            };
        }
    }
}