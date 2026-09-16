using AVMLabs.Api.Services.Models;

namespace AVMLabs.Api.Services.Interfaces
{
    public interface IOutstandingCalculator
    {
        Task<OutstandingAmounts> GetForClientAsync(int clientId);
        Task<Dictionary<int, OutstandingAmounts>> GetForAllClientsAsync();
    }
}