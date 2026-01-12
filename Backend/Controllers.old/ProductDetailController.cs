using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductDetailController : ControllerBase
{
    private readonly IProductDetailService _service;
    public ProductDetailController(IProductDetailService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDetailResponseDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<IEnumerable<ProductDetailResponseDto>>> GetByProductId(int productId)
        => Ok(await _service.GetByProductIdAsync(productId));

    [Authorize(Roles = "admin,employee")]
    [HttpPost]
    public async Task<ActionResult<ProductDetailResponseDto>> Create(ProductDetailCreateDto dto)
        => Ok(await _service.CreateAsync(dto));

    [Authorize(Roles = "admin,employee")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductDetailUpdateDto dto)
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
