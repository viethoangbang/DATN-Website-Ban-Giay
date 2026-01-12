using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountService _service;

    public DiscountController(IDiscountService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DiscountResponseDto>>> GetAll()
    {
        try
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<IEnumerable<DiscountResponseDto>>> GetByProductId(int productId)
    {
        var items = await _service.GetByProductIdAsync(productId);
        return Ok(items);
    }

    [HttpGet("product/{productId}/active")]
    public async Task<ActionResult<IEnumerable<DiscountResponseDto>>> GetActiveByProductId(int productId)
    {
        var items = await _service.GetActiveByProductIdAsync(productId);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DiscountResponseDto>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<DiscountResponseDto>> Create([FromBody] DiscountCreateDto dto)
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

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DiscountUpdateDto dto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}

