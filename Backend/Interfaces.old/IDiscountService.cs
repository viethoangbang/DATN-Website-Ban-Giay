using Business.DTOs;

namespace Business.Interfaces;

public interface IDiscountService
{
    Task<IEnumerable<DiscountResponseDto>> GetAllAsync();
    Task<DiscountResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<DiscountResponseDto>> GetByProductIdAsync(int productId);
    Task<IEnumerable<DiscountResponseDto>> GetActiveByProductIdAsync(int productId);
    Task<DiscountResponseDto> CreateAsync(DiscountCreateDto dto);
    Task<bool> UpdateAsync(int id, DiscountUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

