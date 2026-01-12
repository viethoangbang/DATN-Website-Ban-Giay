using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _service;
    private readonly ICloudinaryService _cloudinaryService;
    
    public ImageController(IImageService service, ICloudinaryService cloudinaryService)
    {
        _service = service;
        _cloudinaryService = cloudinaryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ImageResponseDto>>> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<string>>> GetTypes()
        => Ok(await _service.GetDistinctTypesAsync());

    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<ImageResponseDto>>> GetByType(string type)
        => Ok(await _service.GetByTypeAsync(type));

    [HttpGet("hero")]
    public async Task<ActionResult<IEnumerable<ImageResponseDto>>> GetHeroImages()
        => Ok(await _service.GetByTypeAsync("Hero"));

    [HttpGet("{id}")]
    public async Task<ActionResult<ImageResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = "admin,employee")]
    [HttpPost]
    public async Task<ActionResult<ImageResponseDto>> Create(ImageCreateDto dto)
        => Ok(await _service.CreateAsync(dto));

    [Authorize(Roles = "admin,employee")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ImageUpdateDto dto)
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

    [Authorize]
    [HttpPost("upload")]
    public async Task<ActionResult<ImageResponseDto>> UploadImage(IFormFile file, [FromQuery] string? type = null, [FromQuery] string? folder = null)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded" });
        }

        // Validate file size (max 5MB)
        const long maxFileSize = 5 * 1024 * 1024; // 5MB
        if (file.Length > maxFileSize)
        {
            return BadRequest(new { message = "File size exceeds 5MB limit" });
        }

        // Validate file format
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return BadRequest(new { message = $"File format not allowed. Allowed formats: {string.Join(", ", allowedExtensions)}" });
        }

        try
        {
            // Upload to Cloudinary
            using var stream = file.OpenReadStream();
            var imageUrl = await _cloudinaryService.UploadImageAsync(stream, file.FileName, folder);

            // Save to database
            var createDto = new ImageCreateDto
            {
                Url = imageUrl,
                Type = type ?? "Product",
                Status = "Active"
            };

            var result = await _service.CreateAsync(createDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error uploading image: {ex.Message}" });
        }
    }

    [Authorize(Roles = "admin,employee")]
    [HttpPost("upload-multiple")]
    public async Task<ActionResult<object>> UploadMultipleImages([FromForm] List<IFormFile> files, [FromQuery] string? type = null, [FromQuery] string? folder = null, [FromQuery] int? maxFiles = null)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest(new { message = "No files uploaded" });
        }

        // Use maxFiles parameter if provided, otherwise default to 10
        int maxFileCount = maxFiles ?? 10;
        
        // Validate file count
        if (files.Count > maxFileCount)
        {
            return BadRequest(new { message = $"Maximum {maxFileCount} files allowed per upload" });
        }

        const long maxFileSize = 5 * 1024 * 1024; // 5MB
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var results = new List<ImageResponseDto>();
        var errors = new List<string>();

        for (int i = 0; i < files.Count; i++)
        {
            var file = files[i];
            
            try
            {
                // Validate file size
                if (file.Length > maxFileSize)
                {
                    errors.Add($"{file.FileName}: File size exceeds 5MB limit");
                    continue;
                }

                // Validate file format
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    errors.Add($"{file.FileName}: File format not allowed");
                    continue;
                }

                // Upload to Cloudinary
                using var stream = file.OpenReadStream();
                var imageUrl = await _cloudinaryService.UploadImageAsync(stream, file.FileName, folder);

                // Save to database
                // If type is not provided, default to "Variant" for variant management page
                var imageType = type ?? "Variant";
                var createDto = new ImageCreateDto
                {
                    Url = imageUrl,
                    Type = imageType,
                    Status = "Active"
                };

                var result = await _service.CreateAsync(createDto);
                results.Add(result);
            }
            catch (Exception ex)
            {
                errors.Add($"{file.FileName}: {ex.Message}");
            }
        }

        if (results.Count == 0)
        {
            return StatusCode(500, new { message = "All uploads failed", errors });
        }

        return Ok(new { 
            success = results.Count, 
            failed = errors.Count, 
            images = results, 
            errors = errors.Count > 0 ? errors : null 
        });
    }
}
