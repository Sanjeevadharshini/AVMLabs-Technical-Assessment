using AVMLabs.Api.DTOs.WorkOrders;

namespace AVMLabs.Api.Services.Interfaces
{
    public interface IWorkOrderService
    {
        Task<WorkOrderResponseDto> CreateWorkOrderAsync(CreateWorkOrderDto dto);
        Task<List<InTransitWorkOrderDto>> GetInTransitWorkOrdersAsync();
        Task<WorkOrderResponseDto> UpdateStatusAsync(int woId, UpdateWorkOrderStatusDto dto);
    }
}