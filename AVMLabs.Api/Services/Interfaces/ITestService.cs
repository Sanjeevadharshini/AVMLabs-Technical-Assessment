using AVMLabs.Api.DTOs.Tests;

namespace AVMLabs.Api.Services.Interfaces
{
    public interface ITestService
    {
        Task<List<TestResponseDto>> GetTestsAsync();
    }
}