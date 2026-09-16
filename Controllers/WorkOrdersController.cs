using AVMLabs.Api.DTOs.Common;
using AVMLabs.Api.DTOs.WorkOrders;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AVMLabs.Api.Controllers
{
    [ApiController]
    [Route("api/workorders")]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IWorkOrderService _workOrderService;

        public WorkOrdersController(IWorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WorkOrderResponseDto>>> CreateWorkOrder(CreateWorkOrderDto dto)
        {
            var workOrder = await _workOrderService.CreateWorkOrderAsync(dto);

            return StatusCode(201, ApiResponse<WorkOrderResponseDto>.SuccessResponse(workOrder, "Work order created successfully."));
        }

        [HttpGet("intransit")]
        public async Task<ActionResult<ApiResponse<List<InTransitWorkOrderDto>>>> GetInTransitWorkOrders()
        {
            var workOrders = await _workOrderService.GetInTransitWorkOrdersAsync();

            return Ok(ApiResponse<List<InTransitWorkOrderDto>>.SuccessResponse(workOrders, "In-transit work orders retrieved successfully."));
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ApiResponse<WorkOrderResponseDto>>> UpdateStatus(int id, UpdateWorkOrderStatusDto dto)
        {
            var workOrder = await _workOrderService.UpdateStatusAsync(id, dto);

            return Ok(ApiResponse<WorkOrderResponseDto>.SuccessResponse(workOrder, "Work order status updated successfully."));
        }
    }
}