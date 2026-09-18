using AVMLabs.Api.DTOs.Clients;
using AVMLabs.Api.DTOs.Common;
using AVMLabs.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AVMLabs.Api.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class NblController : ControllerBase
    {
        private readonly INblService _nblService;

        public NblController(INblService nblService)
        {
            _nblService = nblService;
        }

        [HttpGet("{id}/nbl-status")]
        public async Task<ActionResult<ApiResponse<NblStatusDto>>> GetNblStatus(int id)
        {
            var nblStatus = await _nblService.GetNblStatusAsync(id);

            return Ok(ApiResponse<NblStatusDto>.SuccessResponse(nblStatus, "NBL status retrieved successfully."));
        }
    }
}