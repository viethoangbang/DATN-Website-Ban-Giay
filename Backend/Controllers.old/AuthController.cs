using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AccountResponseDto>> Register([FromBody] RegisterDto dto)
    {
        // ModelState validation is handled automatically by [ApiController]
        // If we reach here, ModelState is valid
        
        try
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            
            if (result == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(result);
        }
        catch (Business.Exceptions.UnauthorizedException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("init-admin")]
    public async Task<IActionResult> InitializeAdmin()
    {
        var result = await _authService.InitializeAdminAsync();
        
        if (!result)
            return BadRequest(new { message = "Admin already exists" });

        return Ok(new { message = "Admin account created successfully. Email: admin@shopgiay.com, Password: Admin@123" });
    }

    [Authorize(Roles = "admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok(new { message = "This is admin only endpoint" });
    }

    [Authorize]
    [HttpGet("protected")]
    public IActionResult Protected()
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
        var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
        
        return Ok(new { message = "This is protected endpoint", email, role });
    }
}
