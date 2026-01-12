using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandBannerController : ControllerBase
{
    private readonly IBrandBannerService _service;

    public BrandBannerController(IBrandBannerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BrandBannerResponseDto>>> GetAll()
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

    [HttpGet("brand/{brand}")]
    public async Task<ActionResult<IEnumerable<BrandBannerResponseDto>>> GetByBrand(string brand)
    {
        var items = await _service.GetByBrandAsync(brand);
        return Ok(items);
    }

    [HttpGet("brand/{brand}/active")]
    public async Task<ActionResult<IEnumerable<BrandBannerResponseDto>>> GetActiveByBrand(string brand)
    {
        var items = await _service.GetActiveByBrandAsync(brand);
        return Ok(items);
    }

    [HttpGet("sections")]
    public async Task<ActionResult<IEnumerable<BrandSectionDto>>> GetBrandSections()
    {
        try
        {
            var sections = await _service.GetBrandSectionsAsync();
            return Ok(sections);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BrandBannerResponseDto>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<BrandBannerResponseDto>> Create([FromBody] BrandBannerCreateDto dto)
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
    public async Task<IActionResult> Update(int id, [FromBody] BrandBannerUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (!result) return NotFound();
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<string>>> GetDistinctBrands()
    {
        var brands = await _service.GetDistinctBrandsAsync();
        return Ok(brands);
    }
}

