using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColorController : ControllerBase
{
    private readonly IColorService _service;

    public ColorController(IColorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ColorResponseDto>>> GetAll()
    {
        var colors = await _service.GetAllAsync();
        return Ok(colors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColorResponseDto>> GetById(int id)
    {
        var color = await _service.GetByIdAsync(id);
        if (color == null)
            return NotFound();
        return Ok(color);
    }

    [HttpPost]
    public async Task<ActionResult<ColorResponseDto>> Create(ColorCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ColorUpdateDto dto)
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
