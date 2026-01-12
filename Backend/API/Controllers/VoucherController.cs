using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoucherController : ControllerBase
{
    private readonly IVoucherService _service;
    public VoucherController(IVoucherService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VoucherResponseDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<VoucherResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<VoucherResponseDto>> GetByCode(string code)
    {
        var result = await _service.GetByCodeAsync(code);
        return result == null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = "admin,employee")]
    [HttpPost]
    public async Task<ActionResult<VoucherResponseDto>> Create(VoucherCreateDto dto)
        => Ok(await _service.CreateAsync(dto));

    [Authorize(Roles = "admin,employee")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, VoucherUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
