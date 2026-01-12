using Business.DTOs;
using Business.Interfaces;
using Business.Exceptions;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class BrandBannerService : IBrandBannerService
{
    private readonly IBrandBannerRepo _repo;
    private readonly IImageRepo _imageRepo;
    private readonly IProductRepo _productRepo;

    public BrandBannerService(IBrandBannerRepo repo, IImageRepo imageRepo, IProductRepo productRepo)
    {
        _repo = repo;
        _imageRepo = imageRepo;
        _productRepo = productRepo;
    }

    public async Task<IEnumerable<BrandBannerResponseDto>> GetAllAsync()
    {
        try
        {
            var items = await _repo.GetAllAsync();
            return items.Select(MapToDto);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting brand banners: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<BrandBannerResponseDto>> GetByBrandAsync(string brand)
    {
        var items = await _repo.GetByBrandAsync(brand);
        return items.Select(MapToDto);
    }

    public async Task<IEnumerable<BrandBannerResponseDto>> GetActiveByBrandAsync(string brand)
    {
        var items = await _repo.GetActiveByBrandAsync(brand);
        return items.Select(MapToDto);
    }

    public async Task<BrandBannerResponseDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item == null ? null : MapToDto(item);
    }

    public async Task<BrandBannerResponseDto> CreateAsync(BrandBannerCreateDto dto)
    {
        // Validate image exists
        var image = await _imageRepo.GetByIdAsync(dto.ImageId);
        if (image == null)
            throw new NotFoundException("Image", dto.ImageId);

        var entity = new BrandBanner
        {
            Brand = dto.Brand.Trim(),
            ImageId = dto.ImageId,
            DisplayOrder = dto.DisplayOrder ?? 0,
            Status = dto.Status ?? "Active",
            CreateBy = dto.CreateBy
        };

        var created = await _repo.AddAsync(entity);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, BrandBannerUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        if (dto.ImageId.HasValue)
        {
            var image = await _imageRepo.GetByIdAsync(dto.ImageId.Value);
            if (image == null)
                throw new NotFoundException("Image", dto.ImageId.Value);
            entity.ImageId = dto.ImageId.Value;
        }

        if (!string.IsNullOrWhiteSpace(dto.Brand))
            entity.Brand = dto.Brand.Trim();

        if (dto.DisplayOrder.HasValue)
            entity.DisplayOrder = dto.DisplayOrder.Value;

        if (!string.IsNullOrWhiteSpace(dto.Status))
            entity.Status = dto.Status;

        if (!string.IsNullOrWhiteSpace(dto.UpdateBy))
            entity.UpdateBy = dto.UpdateBy;

        await _repo.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;
        await _repo.DeleteAsync(entity);
        return true;
    }

    public async Task<IEnumerable<string>> GetDistinctBrandsAsync()
    {
        return await _repo.GetDistinctBrandsAsync();
    }

    public async Task<IEnumerable<BrandSectionDto>> GetBrandSectionsAsync()
    {
        try
        {
            // Get all brands that have active banners
            var brandsWithBanners = await _repo.GetDistinctBrandsAsync();
            
            var sections = new List<BrandSectionDto>();

            foreach (var brand in brandsWithBanners)
            {
                // Get active banners for this brand
                var banners = await _repo.GetActiveByBrandAsync(brand);
                
                // Get products for this brand (only active products)
                var allProducts = await _productRepo.GetAllAsync();
                var brandProducts = allProducts
                    .Where(p => p.Brand == brand && p.IsActive)
                    .Take(8) // Limit to 8 products per brand
                    .Select(p => new ProductResponseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        IsActive = p.IsActive,
                        Status = p.Status,
                        Description = p.Description,
                        CreateDate = p.CreateDate,
                        UpdateDate = p.UpdateDate,
                        CreateBy = p.CreateBy,
                        UpdateBy = p.UpdateBy,
                        CategoryId = p.CategoryId,
                        Code = p.Code,
                        Brand = p.Brand
                    })
                    .ToList();

                if (banners.Any() || brandProducts.Any())
                {
                    sections.Add(new BrandSectionDto
                    {
                        Brand = brand,
                        Banners = banners.Select(MapToDto).ToList(),
                        Products = brandProducts
                    });
                }
            }

            return sections;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting brand sections: {ex.Message}", ex);
        }
    }

    private BrandBannerResponseDto MapToDto(BrandBanner entity) => new()
    {
        Id = entity.Id,
        Brand = entity.Brand,
        ImageId = entity.ImageId,
        ImageUrl = entity.Image?.Url,
        DisplayOrder = entity.DisplayOrder,
        Status = entity.Status,
        CreateDate = entity.CreateDate,
        UpdateDate = entity.UpdateDate
    };
}

