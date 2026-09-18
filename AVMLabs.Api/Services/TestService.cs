using AVMLabs.Api.Data;
using AVMLabs.Api.DTOs.Tests;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AVMLabs.Api.Services
{
    public class TestService : ITestService
    {
        private readonly AppDbContext _context;

        public TestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TestResponseDto>> GetTestsAsync()
        {
            return await _context.Tests
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => new TestResponseDto
                {
                    TestId = x.TestId,
                    TestName = x.TestName,
                    Rate = x.Rate,
                    SampleType = x.SampleType,
                    IsOST = x.IsOST
                })
                .ToListAsync();
        }
    }
}