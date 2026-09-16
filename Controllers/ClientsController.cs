using AVMLabs.Api.DTOs.Clients;
using AVMLabs.Api.DTOs.Common;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AVMLabs.Api.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ClientResponseDto>>>> GetClients()
        {
            var clients = await _clientService.GetClientsAsync();

            return Ok(ApiResponse<List<ClientResponseDto>>.SuccessResponse(clients, "Clients retrieved successfully."));
        }

        [HttpGet("{id}/ledger")]
        public async Task<ActionResult<ApiResponse<ClientLedgerDto>>> GetClientLedger(int id)
        {
            var ledger = await _clientService.GetClientLedgerAsync(id);

            return Ok(ApiResponse<ClientLedgerDto>.SuccessResponse(ledger, "Client ledger retrieved successfully."));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ClientResponseDto>>> CreateClient(CreateClientDto dto)
        {
            var client = await _clientService.CreateClientAsync(dto);

            return StatusCode(201, ApiResponse<ClientResponseDto>.SuccessResponse(client, "Client created successfully."));
        }

        [HttpPut("{id}/toggle")]
        public async Task<ActionResult<ApiResponse<ClientResponseDto>>> ToggleClient(int id)
        {
            var client = await _clientService.ToggleClientAsync(id);

            return Ok(ApiResponse<ClientResponseDto>.SuccessResponse(client, "Client status updated successfully."));
        }
    }
}