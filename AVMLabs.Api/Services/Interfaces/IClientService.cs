using AVMLabs.Api.DTOs.Clients;

namespace AVMLabs.Api.Services.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientResponseDto>> GetClientsAsync();
        Task<ClientLedgerDto> GetClientLedgerAsync(int clientId);
        Task<ClientResponseDto> CreateClientAsync(CreateClientDto dto);
        Task<ClientResponseDto> UpdateClientAsync(int clientId, UpdateClientDto dto);
        Task<ClientResponseDto> ToggleClientAsync(int clientId);
    }
}