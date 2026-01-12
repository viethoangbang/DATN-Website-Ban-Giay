using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid request",
                errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });
        }

        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var response = await _authService.LoginAsync(request);

        if (!response.Success)
        {
            _logger.LogWarning("Login failed for email: {Email}", request.Email);
            return Unauthorized(response);
        }

        _logger.LogInformation("Login successful for email: {Email}", request.Email);
        return Ok(response);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new
        {
            message = "Auth endpoint is working",
            timestamp = DateTime.UtcNow
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid request",
                errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });
        }

        _logger.LogInformation("Register attempt for email: {Email}", request.Email);

        var response = await _authService.RegisterAsync(request);

        if (!response.Success)
        {
            _logger.LogWarning("Registration failed for email: {Email}", request.Email);
            return BadRequest(response);
        }

        _logger.LogInformation("Registration successful for email: {Email}", request.Email);
        return Ok(response);
    }
}
