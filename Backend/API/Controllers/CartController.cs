using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _service;
    public CartController(ICartService service) => _service = service;

    [HttpGet("my-cart")]
    public async Task<ActionResult<CartResponseDto>> GetMyCart()
    {
        var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _service.GetMyCartAsync(accountId);
        return result == null ? Ok(new CartResponseDto()) : Ok(result);
    }

    [HttpPost("add")]
    public async Task<ActionResult<CartResponseDto>> AddToCart(AddToCartDto dto)
    {
        var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        return Ok(await _service.AddToCartAsync(accountId, dto));
    }

    [HttpPut("item/{cartDetailId}")]
    public async Task<IActionResult> UpdateCartItem(int cartDetailId, UpdateCartItemDto dto)
    {
        var result = await _service.UpdateCartItemAsync(cartDetailId, dto);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("item/{cartDetailId}")]
    public async Task<IActionResult> RemoveFromCart(int cartDetailId)
    {
        var result = await _service.RemoveFromCartAsync(cartDetailId);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart()
    {
        var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _service.ClearCartAsync(accountId);
        return result ? NoContent() : NotFound();
    }
}
