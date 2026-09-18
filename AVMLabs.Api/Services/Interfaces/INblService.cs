using AVMLabs.Api.DTOs.Clients;

namespace AVMLabs.Api.Services.Interfaces
{
    public interface INblService
    {
        Task<NblStatusDto> GetNblStatusAsync(int clientId);
    }
}