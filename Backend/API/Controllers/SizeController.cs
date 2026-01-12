using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SizeController : ControllerBase
{
    private readonly ISizeService _service;

    public SizeController(ISizeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SizeResponseDto>>> GetAll()
    {
        var sizes = await _service.GetAllAsync();
        return Ok(sizes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SizeResponseDto>> GetById(int id)
    {
        var size = await _service.GetByIdAsync(id);
        if (size == null)
            return NotFound();
        return Ok(size);
    }

    [HttpPost]
    public async Task<ActionResult<SizeResponseDto>> Create(SizeCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SizeUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
