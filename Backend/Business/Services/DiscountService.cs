using Business.DTOs;
using Business.Interfaces;
using Business.Exceptions;
using Data.Interfaces;
using Data.Models;

namespace Business.Services;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepo _repo;
    private readonly IProductRepo _productRepo;

    public DiscountService(IDiscountRepo repo, IProductRepo productRepo)
    {
        _repo = repo;
        _productRepo = productRepo;
    }

    public async Task<IEnumerable<DiscountResponseDto>> GetAllAsync()
    {
        try
        {
            var discounts = await _repo.GetAllAsync();
            return discounts.Select(MapToDto);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting discounts: {ex.Message}", ex);
        }
    }

    public async Task<DiscountResponseDto?> GetByIdAsync(int id)
    {
        var discount = await _repo.GetByIdAsync(id);
        return discount == null ? null : MapToDto(discount);
    }

    public async Task<IEnumerable<DiscountResponseDto>> GetByProductIdAsync(int productId)
    {
        var discounts = await _repo.GetByProductIdAsync(productId);
        return discounts.Select(MapToDto);
    }

    public async Task<IEnumerable<DiscountResponseDto>> GetActiveByProductIdAsync(int productId)
    {
        var discounts = await _repo.GetActiveByProductIdAsync(productId);
        return discounts.Select(MapToDto);
    }

    public async Task<DiscountResponseDto> CreateAsync(DiscountCreateDto dto)
    {
        // Validate product exists
        var product = await _productRepo.GetByIdAsync(dto.ProductId);
        if (product == null)
            throw new NotFoundException("Product", dto.ProductId);

        // Validate dates
        if (dto.EndDate < dto.StartDate)
            throw new BadRequestException("End date must be after start date");

        // Validate discount value
        if (dto.DiscountType == "Percentage" && (dto.DiscountValue < 0 || dto.DiscountValue > 100))
            throw new BadRequestException("Percentage discount must be between 0 and 100");

        var entity = new Discount
        {
            ProductId = dto.ProductId,
            DiscountType = dto.DiscountType,
            DiscountValue = dto.DiscountValue,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status ?? "Active",
            CreateBy = dto.CreateBy,
            CreateDate = DateTime.Now
        };

        var created = await _repo.AddAsync(entity);
        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(int id, DiscountUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return false;

        if (dto.ProductId.HasValue)
        {
            var product = await _productRepo.GetByIdAsync(dto.ProductId.Value);
            if (product == null)
                throw new NotFoundException("Product", dto.ProductId.Value);
            entity.ProductId = dto.ProductId.Value;
        }

        if (!string.IsNullOrWhiteSpace(dto.DiscountType))
            entity.DiscountType = dto.DiscountType;

        if (dto.DiscountValue.HasValue)
        {
            if (entity.DiscountType == "Percentage" && (dto.DiscountValue.Value < 0 || dto.DiscountValue.Value > 100))
                throw new BadRequestException("Percentage discount must be between 0 and 100");
            entity.DiscountValue = dto.DiscountValue.Value;
        }

        if (dto.StartDate.HasValue)
            entity.StartDate = dto.StartDate.Value;

        if (dto.EndDate.HasValue)
        {
            var startDate = dto.StartDate ?? entity.StartDate;
            if (dto.EndDate.Value < startDate)
                throw new BadRequestException("End date must be after start date");
            entity.EndDate = dto.EndDate.Value;
        }

        if (!string.IsNullOrWhiteSpace(dto.Status))
            entity.Status = dto.Status;

        if (!string.IsNullOrWhiteSpace(dto.UpdateBy))
            entity.UpdateBy = dto.UpdateBy;

        entity.UpdateDate = DateTime.Now;

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

    private DiscountResponseDto MapToDto(Discount entity) => new()
    {
        Id = entity.Id,
        ProductId = entity.ProductId,
        ProductName = entity.Product?.Name,
        DiscountType = entity.DiscountType,
        DiscountValue = entity.DiscountValue,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Status = entity.Status,
        CreateBy = entity.CreateBy,
        CreateDate = entity.CreateDate,
        UpdateBy = entity.UpdateBy,
        UpdateDate = entity.UpdateDate
    };
}

