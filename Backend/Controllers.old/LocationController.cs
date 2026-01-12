using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication to use location API
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet("provinces")]
    public async Task<ActionResult<object>> GetProvinces()
    {
        try
        {
            var result = await _locationService.GetProvincesAsync();
            return Ok(result);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(502, new { message = $"External API error: {ex.Message}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                message = $"Error fetching provinces: {ex.Message}",
                details = ex.InnerException?.Message 
            });
        }
    }

    [HttpGet("provinces/{provinceCode}/districts")]
    public async Task<ActionResult<object>> GetDistricts(string provinceCode)
    {
        try
        {
            if (string.IsNullOrEmpty(provinceCode))
            {
                return BadRequest(new { message = "Province code is required" });
            }

            var result = await _locationService.GetDistrictsAsync(provinceCode);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error fetching districts: {ex.Message}" });
        }
    }

    [HttpGet("districts/{districtCode}/wards")]
    public async Task<ActionResult<object>> GetWards(string districtCode)
    {
        try
        {
            if (string.IsNullOrEmpty(districtCode))
            {
                return BadRequest(new { message = "District code is required" });
            }

            var result = await _locationService.GetWardsAsync(districtCode);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error fetching wards: {ex.Message}" });
        }
    }
}

