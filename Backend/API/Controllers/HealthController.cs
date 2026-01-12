using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly SneakerShopContext _context;

    public HealthController(SneakerShopContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "OK",
            message = "API is running with new database schema",
            timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("database")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (!canConnect)
            {
                return StatusCode(500, new { status = "Error", message = "Cannot connect to database" });
            }

            var brands = await _context.Brands.CountAsync();
            var categories = await _context.Categories.CountAsync();
            var products = await _context.Products.CountAsync();
            var accounts = await _context.Accounts.CountAsync();
            var roles = await _context.Roles.CountAsync();

            return Ok(new
            {
                status = "OK",
                message = "Database connection successful",
                database = "SneakerShop",
                tables = new
                {
                    brands,
                    categories,
                    products,
                    accounts,
                    roles
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "Error",
                message = "Database connection failed",
                error = ex.Message
            });
        }
    }

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        try
        {
            var brands = await _context.Brands
                .OrderBy(b => b.Name)
                .ToListAsync();

            return Ok(new
            {
                status = "OK",
                message = $"Found {brands.Count} brands",
                data = brands
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "Error",
                message = "Failed to fetch brands",
                error = ex.Message
            });
        }
    }

    [HttpGet("hash-password/{password}")]
    public IActionResult HashPassword(string password)
    {
        try
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(password, 11);
            return Ok(new
            {
                status = "OK",
                password = password,
                hashed = hashed
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "Error",
                error = ex.Message
            });
        }
    }
}
