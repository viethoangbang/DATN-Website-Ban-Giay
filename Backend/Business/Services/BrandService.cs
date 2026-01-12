using Business.DTOs;
using Business.Interfaces;
using Business.Exceptions;
using Data.Interfaces;
using Data.Models;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepo _repo;
    private readonly SneakerShopContext _context;

    public BrandService(IBrandRepo repo, SneakerShopContext context)
    {
        _repo = repo;
        _context = context;
    }

    public async Task<IEnumerable<BrandResponseDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return items.Select(MapToDto);
    }

    public async Task<BrandResponseDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item == null ? null : MapToDto(item);
    }

    public async Task<BrandResponseDto?> GetByNameAsync(string name)
    {
        var item = await _repo.GetByNameAsync(name);
        return item == null ? null : MapToDto(item);
    }

    public async Task<IEnumerable<BrandResponseDto>> GetActiveBrandsAsync()
    {
        var items = await _repo.GetActiveBrandsAsync();
        return items.Select(MapToDto);
    }

    public async Task<BrandResponseDto> CreateAsync(BrandCreateDto dto)
    {
        // Check if brand with same name already exists
        var existing = await _repo.GetByNameAsync(dto.Name.Trim());
        if (existing != null)
            throw new BadRequestException($"Brand với tên '{dto.Name}' đã tồn tại");

        var entity = new Brand
        {
            Name = dto.Name.Trim(),
            Description = dto.Description,
            LogoUrl = dto.LogoUrl,
            Status = dto.Status ?? "Active",
            DisplayOrder = dto.DisplayOrder ?? 0,
            CreateBy = dto.CreateBy,
            CreateDate = DateTime.Now
        };

        var created = await _repo.AddAsync(entity);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, BrandUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        // Check name uniqueness if name is being updated
        if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name.Trim() != entity.Name)
        {
            var existing = await _repo.GetByNameAsync(dto.Name.Trim());
            if (existing != null && existing.Id != id)
                throw new BadRequestException($"Brand với tên '{dto.Name}' đã tồn tại");
            
            entity.Name = dto.Name.Trim();
        }

        if (dto.Description != null) entity.Description = dto.Description;
        if (dto.LogoUrl != null) entity.LogoUrl = dto.LogoUrl;
        if (dto.Status != null) entity.Status = dto.Status;
        if (dto.DisplayOrder.HasValue) entity.DisplayOrder = dto.DisplayOrder;
        if (dto.UpdateBy != null) entity.UpdateBy = dto.UpdateBy;
        
        entity.UpdateDate = DateTime.Now;

        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        
        // Check if brand is being used by products
        var hasProducts = await _context.Products
            .AnyAsync(p => p.BrandId == id);
        
        // Check if brand is being used by banners
        var hasBanners = await _context.BrandBanners
            .AnyAsync(bb => bb.BrandId == id);
        
        if (hasProducts || hasBanners)
        {
            throw new BadRequestException("Không thể xóa brand này vì đang được sử dụng bởi sản phẩm hoặc banner");
        }
        
        await _repo.DeleteAsync(entity);
        return true;
    }

    private BrandResponseDto MapToDto(Brand entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        LogoUrl = entity.LogoUrl,
        Status = entity.Status,
        DisplayOrder = entity.DisplayOrder,
        CreateBy = entity.CreateBy,
        CreateDate = entity.CreateDate,
        UpdateBy = entity.UpdateBy,
        UpdateDate = entity.UpdateDate
    };
}
