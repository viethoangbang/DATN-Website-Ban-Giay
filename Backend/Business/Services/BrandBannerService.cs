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
    private readonly IBrandRepo _brandRepo;

    public BrandBannerService(IBrandBannerRepo repo, IImageRepo imageRepo, IProductRepo productRepo, IBrandRepo brandRepo)
    {
        _repo = repo;
        _imageRepo = imageRepo;
        _productRepo = productRepo;
        _brandRepo = brandRepo;
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

        // Validate BrandId if provided
        string brandName = null!;
        if (dto.BrandId.HasValue)
        {
            var brand = await _brandRepo.GetByIdAsync(dto.BrandId.Value);
            if (brand == null)
                throw new NotFoundException("Brand", dto.BrandId.Value);
            brandName = brand.Name;
        }
        else if (!string.IsNullOrWhiteSpace(dto.Brand))
        {
            brandName = dto.Brand.Trim();
            // Try to find brand by name and set BrandId
            var brand = await _brandRepo.GetByNameAsync(brandName);
            if (brand != null)
            {
                dto.BrandId = brand.Id;
            }
        }
        else
        {
            throw new BadRequestException("Brand hoặc BrandId phải được cung cấp");
        }

        var entity = new BrandBanner
        {
            Brand = brandName, // Keep for backward compatibility
            BrandId = dto.BrandId,
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

        if (dto.BrandId.HasValue)
        {
            // Validate BrandId exists
            var brand = await _brandRepo.GetByIdAsync(dto.BrandId.Value);
            if (brand == null)
                throw new NotFoundException("Brand", dto.BrandId.Value);
            entity.BrandId = dto.BrandId.Value;
            entity.Brand = brand.Name; // Update Brand string to match BrandId
        }
        else if (!string.IsNullOrWhiteSpace(dto.Brand))
        {
            entity.Brand = dto.Brand.Trim();
            // Try to find brand by name and set BrandId
            var brand = await _brandRepo.GetByNameAsync(dto.Brand.Trim());
            if (brand != null)
            {
                entity.BrandId = brand.Id;
            }
        }

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
                    .Where(p => (p.BrandNavigation != null && p.BrandNavigation.Name == brand) || p.Brand == brand && p.IsActive)
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
                        Brand = p.BrandNavigation?.Name ?? p.Brand,
                        BrandId = p.BrandId,
                        BrandName = p.BrandNavigation?.Name ?? p.Brand
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
        Brand = entity.BrandNavigation?.Name ?? entity.Brand, // Use BrandNavigation if available
        BrandId = entity.BrandId,
        ImageId = entity.ImageId,
        ImageUrl = entity.Image?.Url,
        DisplayOrder = entity.DisplayOrder,
        Status = entity.Status,
        CreateDate = entity.CreateDate,
        UpdateDate = entity.UpdateDate
    };
}

