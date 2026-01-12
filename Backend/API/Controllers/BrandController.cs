using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _service;
    
    public BrandController(IBrandService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetActive()
    {
        return Ok(await _service.GetActiveBrandsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BrandResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<BrandResponseDto>> GetByName(string name)
    {
        var result = await _service.GetByNameAsync(name);
        return result == null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = "admin,employee")]
    [HttpPost]
    public async Task<ActionResult<BrandResponseDto>> Create(BrandCreateDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "admin,employee")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BrandUpdateDto dto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto);
            return result ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "admin,employee")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
