using AVMLabs.Api.DTOs.Common;
using AVMLabs.Api.DTOs.Tests;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AVMLabs.Api.Controllers
{
    [ApiController]
    [Route("api/tests")]
    public class TestsController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestsController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTests()
        {
            var tests = await _testService.GetTestsAsync();
            return Ok(ApiResponse<List<TestResponseDto>>.SuccessResponse(tests, "Tests retrieved successfully."));
        }
    }
}