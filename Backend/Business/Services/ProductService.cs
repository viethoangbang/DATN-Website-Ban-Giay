using Business.DTOs;
using Business.Interfaces;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class ProductService : IProductService
{
    private readonly IProductRepo _repository;

    public ProductService(IProductRepo repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(p => new ProductResponseDto
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
            Brand = p.BrandNavigation?.Name ?? p.Brand, // Use BrandNavigation if available, fallback to Brand string
            BrandId = p.BrandId,
            BrandName = p.BrandNavigation?.Name ?? p.Brand,
            Weight = p.Weight
        });
    }

    public async Task<IEnumerable<ProductResponseDto>> GetNewProductsAsync(int limit = 10)
    {
        var products = await _repository.GetAllAsync();
        return products
            .Where(p => p.IsActive && p.Status == "New") // Only active products with "New" badge
            .OrderByDescending(p => p.CreateDate ?? DateTime.MinValue)
            .Take(limit)
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
                BrandName = p.BrandNavigation?.Name ?? p.Brand,
                Weight = p.Weight
            });
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return null;

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            IsActive = product.IsActive,
            Status = product.Status,
            Description = product.Description,
            CreateDate = product.CreateDate,
            UpdateDate = product.UpdateDate,
            CreateBy = product.CreateBy,
            UpdateBy = product.UpdateBy,
            CategoryId = product.CategoryId,
            Code = product.Code,
            Brand = product.BrandNavigation?.Name ?? product.Brand,
            BrandId = product.BrandId,
            BrandName = product.BrandNavigation?.Name ?? product.Brand,
            Weight = product.Weight
        };
    }

    public async Task<ProductResponseDto> CreateAsync(ProductCreateDto dto)
    {
        // Set default status to "New" if not provided
        var status = dto.Status ?? "New";
        
        // If setting status to "New", check if we already have 10 "New" products
        if (status == "New")
        {
            var allProducts = await _repository.GetAllAsync();
            var newProducts = allProducts
                .Where(p => p.Status == "New")
                .OrderBy(p => p.CreateDate ?? DateTime.MaxValue) // Oldest first
                .ToList();
            
            // If we have 10 or more "New" products, remove "New" from the oldest one
            if (newProducts.Count >= 10)
            {
                var oldestNewProduct = newProducts.First();
                oldestNewProduct.Status = null;
                oldestNewProduct.UpdateDate = DateTime.Now;
                await _repository.UpdateAsync(oldestNewProduct);
            }
        }
        
        var product = new Product
        {
            Name = dto.Name,
            IsActive = dto.IsActive,
            Status = status,
            Description = dto.Description,
            CreateBy = dto.CreateBy,
            CategoryId = dto.CategoryId,
            Code = dto.Code,
            Brand = dto.Brand, // Keep for backward compatibility
            BrandId = dto.BrandId, // Use BrandId if provided
            Weight = dto.Weight,
            CreateDate = DateTime.Now
        };

        var created = await _repository.AddAsync(product);
        return new ProductResponseDto
        {
            Id = created.Id,
            Name = created.Name,
            IsActive = created.IsActive,
            Status = created.Status,
            Description = created.Description,
            CreateDate = created.CreateDate,
            CreateBy = created.CreateBy,
            CategoryId = created.CategoryId,
            Code = created.Code,
            Brand = created.BrandNavigation?.Name ?? created.Brand,
            BrandId = created.BrandId,
            BrandName = created.BrandNavigation?.Name ?? created.Brand,
            Weight = created.Weight
        };
    }

    public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return false;

        if (dto.Name != null) product.Name = dto.Name;
        if (dto.IsActive.HasValue) product.IsActive = dto.IsActive.Value;
        if (dto.Status != null) product.Status = dto.Status;
        if (dto.Description != null) product.Description = dto.Description;
        if (dto.UpdateBy != null) product.UpdateBy = dto.UpdateBy;
        if (dto.CategoryId.HasValue) product.CategoryId = dto.CategoryId;
        if (dto.Code != null) product.Code = dto.Code;
        if (dto.Brand != null) product.Brand = dto.Brand; // Keep for backward compatibility
        if (dto.BrandId.HasValue) product.BrandId = dto.BrandId;
        if (dto.Weight.HasValue) product.Weight = dto.Weight.Value;
        product.UpdateDate = DateTime.Now;

        await _repository.UpdateAsync(product);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return false;

        await _repository.DeleteAsync(product);
        return true;
    }
}
