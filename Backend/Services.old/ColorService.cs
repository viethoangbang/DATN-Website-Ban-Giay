using Business.DTOs;
using Business.Interfaces;
using Business.Exceptions;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class ColorService : IColorService
{
    private readonly IColorRepo _repository;

    public ColorService(IColorRepo repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ColorResponseDto>> GetAllAsync()
    {
        var colors = await _repository.GetAllAsync();
        return colors.Select(c => new ColorResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Status = c.Status,
            CreateDate = c.CreateDate,
            UpdateBy = c.UpdateBy,
            UpdateDate = c.UpdateDate
        });
    }

    public async Task<ColorResponseDto?> GetByIdAsync(int id)
    {
        var color = await _repository.GetByIdAsync(id);
        if (color == null) return null;

        return new ColorResponseDto
        {
            Id = color.Id,
            Name = color.Name,
            Status = color.Status,
            CreateDate = color.CreateDate,
            UpdateBy = color.UpdateBy,
            UpdateDate = color.UpdateDate
        };
    }

    public async Task<ColorResponseDto> CreateAsync(ColorCreateDto dto)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new BadRequestException("Tên màu không được để trống");

        // Check for duplicate name (case-insensitive)
        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing != null)
            throw new BadRequestException($"Màu '{dto.Name}' đã tồn tại");

        var color = new Color
        {
            Name = dto.Name.Trim(),
            Status = dto.Status ?? "Active",
            CreateDate = DateTime.Now
        };

        var created = await _repository.AddAsync(color);
        return new ColorResponseDto
        {
            Id = created.Id,
            Name = created.Name,
            Status = created.Status,
            CreateDate = created.CreateDate
        };
    }

    public async Task<bool> UpdateAsync(int id, ColorUpdateDto dto)
    {
        var color = await _repository.GetByIdAsync(id);
        if (color == null) return false;

        // Validate required fields
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new BadRequestException("Tên màu không được để trống");

        // Check for duplicate name (case-insensitive, exclude current)
        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            var existing = await _repository.GetByNameAsync(dto.Name, id);
            if (existing != null)
                throw new BadRequestException($"Màu '{dto.Name}' đã tồn tại");
        }

        color.Name = dto.Name.Trim();
        color.Status = dto.Status;
        color.UpdateBy = dto.UpdateBy;
        color.UpdateDate = DateTime.Now;

        await _repository.UpdateAsync(color);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var color = await _repository.GetByIdAsync(id);
        if (color == null) return false;

        await _repository.DeleteAsync(color);
        return true;
    }
}
