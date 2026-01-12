using Business.DTOs;

namespace Business.Interfaces;

public interface IBrandService
{
    Task<IEnumerable<BrandResponseDto>> GetAllAsync();
    Task<BrandResponseDto?> GetByIdAsync(int id);
    Task<BrandResponseDto?> GetByNameAsync(string name);
    Task<IEnumerable<BrandResponseDto>> GetActiveBrandsAsync();
    Task<BrandResponseDto> CreateAsync(BrandCreateDto dto);
    Task<bool> UpdateAsync(int id, BrandUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
