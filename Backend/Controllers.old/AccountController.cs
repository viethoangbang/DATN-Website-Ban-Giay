using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _service;
    
    public AccountController(IAccountService service)
    {
        _service = service;
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountResponseDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    // Allow users to get their own account info
    [HttpGet("me")]
    public async Task<ActionResult<AccountResponseDto>> GetMyAccount()
    {
        var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        if (accountId == 0)
            return Unauthorized();
            
        var result = await _service.GetByIdAsync(accountId);
        return result == null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AccountResponseDto>> Create(AccountCreateDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AccountUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    [HttpPut("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, AccountChangePasswordDto dto)
    {
        try
        {
            var result = await _service.ChangePasswordAsync(id, dto);
            return result ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}

