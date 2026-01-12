using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GHNController : ControllerBase
{
    private readonly IGHNService _ghnService;
    private readonly ILogger<GHNController> _logger;

    public GHNController(
        IGHNService ghnService,
        ILogger<GHNController> logger)
    {
        _ghnService = ghnService;
        _logger = logger;
    }

    /// <summary>
    /// Get all shops from GHN (for admin to verify ShopId)
    /// </summary>
    [HttpGet("shops")]
    public async Task<IActionResult> GetShops()
    {
        try
        {
            var shops = await _ghnService.GetShopsAsync();
            return Ok(new 
            { 
                success = true,
                shops = shops,
                message = shops.Count > 0 
                    ? $"Tìm thấy {shops.Count} shop(s). Kiểm tra ShopId trong appsettings.json có khớp với shopId trong danh sách này không."
                    : "Không tìm thấy shop nào. Kiểm tra lại Token trong appsettings.json."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching GHN shops");
            return StatusCode(500, new { 
                success = false,
                message = "Error fetching shops",
                error = ex.Message 
            });
        }
    }
}

