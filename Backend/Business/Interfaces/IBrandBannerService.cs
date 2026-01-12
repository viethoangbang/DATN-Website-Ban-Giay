using Business.DTOs;

namespace Business.Interfaces;

public interface IBrandBannerService
{
    Task<IEnumerable<BrandBannerResponseDto>> GetAllAsync();
    Task<IEnumerable<BrandBannerResponseDto>> GetByBrandAsync(string brand);
    Task<IEnumerable<BrandBannerResponseDto>> GetActiveByBrandAsync(string brand);
    Task<BrandBannerResponseDto?> GetByIdAsync(int id);
    Task<BrandBannerResponseDto> CreateAsync(BrandBannerCreateDto dto);
    Task<bool> UpdateAsync(int id, BrandBannerUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<string>> GetDistinctBrandsAsync();
    Task<IEnumerable<BrandSectionDto>> GetBrandSectionsAsync();
}

