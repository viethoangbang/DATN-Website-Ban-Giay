using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GHNLocationController : ControllerBase
{
    private readonly IGHNLocationService _ghnLocationService;
    private readonly ILogger<GHNLocationController> _logger;

    public GHNLocationController(
        IGHNLocationService ghnLocationService,
        ILogger<GHNLocationController> logger)
    {
        _ghnLocationService = ghnLocationService;
        _logger = logger;
    }

    /// <summary>
    /// Get all provinces from GHN
    /// </summary>
    [HttpGet("provinces")]
    public async Task<IActionResult> GetProvinces()
    {
        try
        {
            var provinces = await _ghnLocationService.GetProvincesAsync();
            return Ok(provinces);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching GHN provinces");
            return StatusCode(500, new { message = "Error fetching provinces" });
        }
    }

    /// <summary>
    /// Get districts by province ID
    /// </summary>
    [HttpGet("districts/{provinceId}")]
    public async Task<IActionResult> GetDistricts(int provinceId)
    {
        try
        {
            var districts = await _ghnLocationService.GetDistrictsAsync(provinceId);
            return Ok(districts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching GHN districts for province {provinceId}");
            return StatusCode(500, new { message = "Error fetching districts" });
        }
    }

    /// <summary>
    /// Get wards by district ID
    /// </summary>
    [HttpGet("wards/{districtId}")]
    public async Task<IActionResult> GetWards(int districtId)
    {
        try
        {
            var wards = await _ghnLocationService.GetWardsAsync(districtId);
            return Ok(wards);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching GHN wards for district {districtId}");
            return StatusCode(500, new { message = "Error fetching wards" });
        }
    }
}

