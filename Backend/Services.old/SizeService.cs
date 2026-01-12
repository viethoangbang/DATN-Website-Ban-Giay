using Business.DTOs;
using Business.Interfaces;
using Business.Exceptions;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class SizeService : ISizeService
{
    private readonly ISizeRepo _repository;

    public SizeService(ISizeRepo repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SizeResponseDto>> GetAllAsync()
    {
        var sizes = await _repository.GetAllAsync();
        return sizes.Select(s => new SizeResponseDto
        {
            Id = s.Id,
            SizeName = s.SizeName,
            Quantity = s.Quantity
        });
    }

    public async Task<SizeResponseDto?> GetByIdAsync(int id)
    {
        var size = await _repository.GetByIdAsync(id);
        if (size == null) return null;

        return new SizeResponseDto
        {
            Id = size.Id,
            SizeName = size.SizeName,
            Quantity = size.Quantity
        };
    }

    public async Task<SizeResponseDto> CreateAsync(SizeCreateDto dto)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(dto.SizeName))
            throw new BadRequestException("Tên size không được để trống");

        // Check for duplicate size name (case-insensitive)
        var existing = await _repository.GetBySizeNameAsync(dto.SizeName);
        if (existing != null)
            throw new BadRequestException($"Size '{dto.SizeName}' đã tồn tại");

        var size = new Size
        {
            SizeName = dto.SizeName.Trim(),
            Quantity = dto.Quantity
        };

        var created = await _repository.AddAsync(size);
        return new SizeResponseDto
        {
            Id = created.Id,
            SizeName = created.SizeName,
            Quantity = created.Quantity
        };
    }

    public async Task<bool> UpdateAsync(int id, SizeUpdateDto dto)
    {
        var size = await _repository.GetByIdAsync(id);
        if (size == null) return false;

        // Validate required fields
        if (string.IsNullOrWhiteSpace(dto.SizeName))
            throw new BadRequestException("Tên size không được để trống");

        // Check for duplicate size name (case-insensitive, exclude current)
        if (!string.IsNullOrWhiteSpace(dto.SizeName))
        {
            var existing = await _repository.GetBySizeNameAsync(dto.SizeName, id);
            if (existing != null)
                throw new BadRequestException($"Size '{dto.SizeName}' đã tồn tại");
        }

        size.SizeName = dto.SizeName.Trim();
        size.Quantity = dto.Quantity;

        await _repository.UpdateAsync(size);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var size = await _repository.GetByIdAsync(id);
        if (size == null) return false;

        await _repository.DeleteAsync(size);
        return true;
    }
}
